using System.Data.SqlClient;
using apbd_5_s22085.DAL;
using apbd_5_s22085.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_5_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class Warehouses2Controller : ControllerBase
{

    private readonly ILogger<WarehousesController> _logger;
    private readonly IProceduralWarehouseRepository _proceduralWarehouseRepository;

    public Warehouses2Controller(
        ILogger<WarehousesController> logger,
        IProceduralWarehouseRepository proceduralWarehouseRepository)
    {
        _logger = logger;
        _proceduralWarehouseRepository = proceduralWarehouseRepository;
    }

    [HttpPost(Name = "Register product in warehouse 2")]
    public async Task<IActionResult> RegisterProductInWarehouse(ProductInWarehouseRequestDto requestDto)
    {
        _logger.LogInformation("RegisterProductInWarehouse called with requestDto = {requestDto}", requestDto);
        try
        {
            var id = await _proceduralWarehouseRepository.InsertProductWarehouse(requestDto);
            return Ok(new
            {
                message = "Product registered in warehouse",
                id
            });
        } catch (SqlException e)
        {
            return BadRequest(e.Message);
        }
        
        
        
    }

}