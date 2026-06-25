using server.Models;

namespace server.Services;

public interface IMedicineService
{
    Task<List<Medicine>> GetAllAsync(string? search = null);
    Task<Medicine> AddAsync(Medicine medicine);
    Task<Medicine?> SellAsync(Guid id, int quantity);
}
