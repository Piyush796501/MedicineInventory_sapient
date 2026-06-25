using server.Models;

namespace server.Services;

public interface ISaleService
{
    // Sells the medicine (reduces stock) AND records the sale. Null if medicine not found.
    Task<Sale?> RecordSaleAsync(Guid medicineId, int quantity);

    // Full sales history, newest first.
    Task<List<Sale>> GetAllAsync();
}
