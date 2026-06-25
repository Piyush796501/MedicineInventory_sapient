namespace server.Models;

public class Medicine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = "";
    public string? Notes { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }   // money, 2 decimals
    public string Brand { get; set; } = "";
}
