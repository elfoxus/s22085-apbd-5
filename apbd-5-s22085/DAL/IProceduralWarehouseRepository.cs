using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IProceduralWarehouseRepository
{
    Task<int> InsertProductWarehouse(ProductInWarehouseRequestDto dto);
}