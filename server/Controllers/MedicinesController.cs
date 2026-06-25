using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly IMedicineService _svc;
    private readonly ISaleService _sales;

    public MedicinesController(IMedicineService svc, ISaleService sales)
    {
        _svc = svc;
        _sales = sales;
    }

    // GET /api/medicines?search=para
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? search)
        => Ok(await _svc.GetAllAsync(search));

    // POST /api/medicines
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Medicine m)
    {
        if (string.IsNullOrWhiteSpace(m.FullName) || m.Price < 0 || m.Quantity < 0)
            return BadRequest("Name is required; price and quantity must be non-negative.");

        var created = await _svc.AddAsync(m);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    // POST /api/medicines/{id}/sell   body: { "quantity": 1 }
    // Reduces stock AND records the sale in the sales history.
    [HttpPost("{id}/sell")]
    public async Task<IActionResult> Sell(Guid id, [FromBody] SellRequest req)
    {
        try
        {
            var sale = await _sales.RecordSaleAsync(id, req.Quantity);
            return sale is null ? NotFound() : Ok(sale);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record SellRequest(int Quantity);
