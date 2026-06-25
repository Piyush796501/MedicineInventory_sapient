using System.Text.Json;
using server.Models;

namespace server.Repositories;

// Stores the sales history as a JSON file on the server.
public class JsonSaleRepository : ISaleRepository
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts =
        new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    public JsonSaleRepository(IWebHostEnvironment env)
    {
        _path = Path.Combine(env.ContentRootPath, "Data", "sales.json");
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
    }

    public async Task<List<Sale>> GetAllAsync()
    {
        var json = await File.ReadAllTextAsync(_path);
        return JsonSerializer.Deserialize<List<Sale>>(json, _opts) ?? new();
    }

    public async Task SaveAllAsync(List<Sale> sales) =>
        await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(sales, _opts));
}
