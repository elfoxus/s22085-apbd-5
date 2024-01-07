using System.Data.SqlClient;

namespace apbd_5_s22085.DAL;

public class WarehouseRepository : IWarehouseRepository
{
    
    private readonly string _connectionString;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");
    }
    
    public async Task<bool> CheckIfWarehouseExistsAsync(int warehouseId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("SELECT * FROM Warehouse WHERE IdWarehouse = @warehouseId", connection);
        command.Parameters.AddWithValue("warehouseId", warehouseId);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
}