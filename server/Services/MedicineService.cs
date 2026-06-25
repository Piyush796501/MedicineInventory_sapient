using server.Models;
using server.Repositories;

namespace server.Services;

// Business rules live here: search filtering, validation, sell logic.
// Persistence is delegated to the repository — this class never touches a file.
public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _repo;
    private readonly SemaphoreSlim _lock = new(1, 1);  // make read-modify-write atomic

    public MedicineService(IMedicineRepository repo) => _repo = repo;

    public async Task<List<Medicine>> GetAllAsync(string? search = null)
    {
        var list = await _repo.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
            list = list
                .Where(m => m.FullName.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        return list;
    }

    public async Task<Medicine> AddAsync(Medicine medicine)
    {
        await _lock.WaitAsync();
        try
        {
            var list = await _repo.GetAllAsync();
            medicine.Id = Guid.NewGuid();
            list.Add(medicine);
            await _repo.SaveAllAsync(list);
            return medicine;
        }
        finally { _lock.Release(); }
    }

    public async Task<Medicine?> SellAsync(Guid id, int quantity)
    {
        await _lock.WaitAsync();
        try
        {
            var list = await _repo.GetAllAsync();
            var m = list.FirstOrDefault(x => x.Id == id);
            if (m is null) return null;
            if (quantity <= 0 || quantity > m.Quantity)
                throw new InvalidOperationException("Invalid sell quantity.");
            m.Quantity -= quantity;
            await _repo.SaveAllAsync(list);
            return m;
        }
        finally { _lock.Release(); }
    }
}
