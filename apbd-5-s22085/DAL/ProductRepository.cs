using System.Data.SqlClient;
using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public class ProductRepository : IProductRepository
{
    
    private readonly string _connectionString;
    
    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");
    }
    
    public async Task<ProductPriceDto?> GetPriceIfExists(int productId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT Price FROM Product WHERE IdProduct = {productId}";
            
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        
        await reader.ReadAsync();
        return new ProductPriceDto((decimal)reader["Price"]);
    }
}