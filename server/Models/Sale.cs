namespace server.Models;

// A single sale transaction — the historical record the brief asks us to maintain.
public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }          // price at the time of sale
    public decimal TotalPrice { get; set; }          // UnitPrice * Quantity
    public DateTime SoldAt { get; set; } = DateTime.UtcNow;
}
