using server.Models;

namespace server.Repositories;

// Persistence only — knows how to load and save the medicine list.
// Has no business rules.
public interface IMedicineRepository
{
    Task<List<Medicine>> GetAllAsync();
    Task SaveAllAsync(List<Medicine> medicines);
}
