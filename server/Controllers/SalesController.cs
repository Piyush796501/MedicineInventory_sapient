using Microsoft.AspNetCore.Mvc;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _sales;
    public SalesController(ISaleService sales) => _sales = sales;

    // GET /api/sales  — full sales history, newest first.
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _sales.GetAllAsync());
}
