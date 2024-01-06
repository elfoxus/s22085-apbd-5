namespace apbd_5_s22085.DAL;

public interface IWarehouseRepository
{
    Task<bool> CheckIfWarehouseExistsAsync(int warehouseId);
}