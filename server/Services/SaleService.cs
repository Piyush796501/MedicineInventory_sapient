using server.Models;
using server.Repositories;

namespace server.Services;

// Coordinates a sale: decrement medicine stock (via the medicine service, which
// owns that validation) and persist a Sale record to the sales history.
public class SaleService : ISaleService
{
    private readonly IMedicineService _medicines;
    private readonly ISaleRepository _sales;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public SaleService(IMedicineService medicines, ISaleRepository sales)
    {
        _medicines = medicines;
        _sales = sales;
    }

    public async Task<Sale?> RecordSaleAsync(Guid medicineId, int quantity)
    {
        await _lock.WaitAsync();
        try
        {
            // SellAsync validates stock and throws on invalid quantity.
            var medicine = await _medicines.SellAsync(medicineId, quantity);
            if (medicine is null) return null;   // medicine not found

            var sale = new Sale
            {
                MedicineId = medicine.Id,
                MedicineName = medicine.FullName,
                Quantity = quantity,
                UnitPrice = medicine.Price,
                TotalPrice = medicine.Price * quantity,
                SoldAt = DateTime.UtcNow
            };

            var history = await _sales.GetAllAsync();
            history.Add(sale);
            await _sales.SaveAllAsync(history);
            return sale;
        }
        finally { _lock.Release(); }
    }

    public async Task<List<Sale>> GetAllAsync()
    {
        var history = await _sales.GetAllAsync();
        return history.OrderByDescending(s => s.SoldAt).ToList();
    }
}
