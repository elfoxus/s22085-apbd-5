using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IProductWarehouseRepository
{
    Task FindProductWarehouseByOrderIdAsync(int orderId);

    Task InsertProductWarehouseAsync(NewProductWarehouseDto newProductWarehouseDto);
}