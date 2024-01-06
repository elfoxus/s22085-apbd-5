using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public interface IProductRepository
{
    Task<ProductPriceDto> GetPriceIfExists(int productId);
}