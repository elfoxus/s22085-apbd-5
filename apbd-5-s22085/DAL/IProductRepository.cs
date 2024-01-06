namespace apbd_5_s22085.DAL;

public interface IProductRepository
{
    Task<int> GetProductPriceByIdAsync(int productId);
}