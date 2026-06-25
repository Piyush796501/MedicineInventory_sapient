using server.Models;

namespace server.Repositories;

// Persistence only for the sales history.
public interface ISaleRepository
{
    Task<List<Sale>> GetAllAsync();
    Task SaveAllAsync(List<Sale> sales);
}
