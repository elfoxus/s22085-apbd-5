using apbd_5_s22085.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_5_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WarehousesController : ControllerBase
{

    private readonly ILogger<WarehousesController> _logger;

    public WarehousesController(ILogger<WarehousesController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Register product in warehouse")]
    public async Task<IActionResult> RegisterProductInWarehouse(ProductInWarehouseRequestDto requestDto)
    {
        _logger.LogInformation("RegisterProductInWarehouse called with requestDto = {requestDto}", requestDto);
        return Ok("Product registered in warehouse");
    }
}