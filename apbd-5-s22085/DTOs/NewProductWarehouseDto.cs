namespace apbd_5_s22085.DTOs;

public record NewProductWarehouseDto(
    int warehouseId,
    int productId,
    int orderId,
    int amount, 
    decimal price,
    DateTime createdAt
);