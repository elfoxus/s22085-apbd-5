using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IProductWarehouseRepository
{
    Task<bool> FindProductWarehouseByOrderIdAsync(int orderId);

    Task<int?> InsertProductWarehouseAndUpdateOrderAsync(NewProductWarehouseDto newProductWarehouseDto);
}