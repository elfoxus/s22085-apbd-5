using apbd_5_s22085.DAL;
using apbd_5_s22085.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_5_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WarehousesController : ControllerBase
{

    private readonly ILogger<WarehousesController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;

    public WarehousesController(
        ILogger<WarehousesController> logger,
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository, 
        IOrderRepository orderRepository,
        IProductWarehouseRepository productWarehouseRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _orderRepository = orderRepository;
        _productWarehouseRepository = productWarehouseRepository;
    }

    [HttpPost(Name = "Register product in warehouse")]
    public async Task<IActionResult> RegisterProductInWarehouse(ProductInWarehouseRequestDto requestDto)
    {
        _logger.LogInformation("RegisterProductInWarehouse called with requestDto = {requestDto}", requestDto);

        var productPrice = await CheckIfProductExistsAsync(requestDto.IdProduct);
        if (productPrice == null)
        {
            return NotFound("Product does not exist");
        }
        
        if (!await CheckIfWarehouseExistsAsync(requestDto.IdWarehouse))
        {
            return NotFound("Warehouse does not exist");
        }

        var order = await GetOrderAsync(new FindOrderDto(requestDto.IdProduct, requestDto.Amount, requestDto.CreatedAt));
        if (order == null)
        {
            return NotFound("Order not found");
        }
        
        if (await IsOrderFulfilledAsync(order.OrderId))
        {
            return BadRequest("Product already registered in warehouse");
        }
        
        var id = await _productWarehouseRepository.InsertProductWarehouseAndUpdateOrderAsync(
            new NewProductWarehouseDto(
                requestDto.IdWarehouse,
                requestDto.IdProduct,
                order.OrderId,
                requestDto.Amount,
                requestDto.Amount * productPrice.price,
                DateTime.Now
                ));
        
        if (id == null) return BadRequest("Could not register product in warehouse");
        
        return Ok(new
        {
            message = "Product registered in warehouse",
            id
        });
    }

    private async Task<bool> IsOrderFulfilledAsync(int orderId)
    {
        _logger.LogInformation("Checking if order is fulfilled");
        var result = await _productWarehouseRepository.FindProductWarehouseByOrderIdAsync(orderId);
        
        if (result)
        {
            _logger.LogInformation("Order is fulfilled");
            return true;
        }
        _logger.LogInformation("Order is not fulfilled");
        return false;

    }
    
    private async Task<OrderDto?> GetOrderAsync(FindOrderDto findOrderDto)
    {
        _logger.LogInformation("GetOrderAsync called with findOrderDto = {findOrderDto}", findOrderDto);
        var order = await _orderRepository.FindOrderByProductIdAndAmountAndDateBeforeAsync(findOrderDto);
        if (order == null)
        {
            _logger.LogInformation("Order not found");
            return null;
        }
        _logger.LogInformation("Order found");
        return order;
    }

    private async Task<bool> CheckIfWarehouseExistsAsync(int warehouseId)
    {
        _logger.LogInformation("Checking if warehouse exists");
        var warehouseExists = await _warehouseRepository.CheckIfWarehouseExistsAsync(warehouseId);
        if (!warehouseExists)
        {
            _logger.LogInformation("Warehouse does not exist");
            return false;
        }
        _logger.LogInformation("Warehouse exists");
        return true;
    }
    
    private async Task<ProductPriceDto?> CheckIfProductExistsAsync(int productId)
    {
        _logger.LogInformation("Checking if product exists");
        var productPrice = await _productRepository.GetPriceIfExists(productId);
        if (productPrice == null)
        {
            _logger.LogInformation("Product does not exist");
        }
        else
        {
            _logger.LogInformation("Product exists");
        }

        return productPrice;
    }
}