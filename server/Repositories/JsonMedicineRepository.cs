using System.Text.Json;
using server.Models;

namespace server.Repositories;

// Stores the medicine list as a JSON file on the server.
// Pure data access: read the file, write the file. Nothing else.
public class JsonMedicineRepository : IMedicineRepository
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts =
        new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    public JsonMedicineRepository(IWebHostEnvironment env)
    {
        _path = Path.Combine(env.ContentRootPath, "Data", "medicines.json");
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
    }

    public async Task<List<Medicine>> GetAllAsync()
    {
        var json = await File.ReadAllTextAsync(_path);
        return JsonSerializer.Deserialize<List<Medicine>>(json, _opts) ?? new();
    }

    public async Task SaveAllAsync(List<Medicine> medicines) =>
        await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(medicines, _opts));
}
