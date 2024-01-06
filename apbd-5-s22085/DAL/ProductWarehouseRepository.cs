using System.Data.Common;
using System.Data.SqlClient;
using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly string _connectionString;
    
    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");
    }
    
    public async Task<bool> FindProductWarehouseByOrderIdAsync(int orderId)
    {
        await using SqlConnection connection = new(_connectionString);
        await using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Product_Warehouse WHERE IdOrder = {orderId}";
        
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        return reader.HasRows;
    }

    public async Task<int?> InsertProductWarehouseAndUpdateOrderAsync(NewProductWarehouseDto newProductWarehouseDto)
    {
        await using SqlConnection connection = new(_connectionString);
        await using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = $"INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt)";
        command.Parameters.AddWithValue("idWarehouse", newProductWarehouseDto.warehouseId);
        command.Parameters.AddWithValue("idProduct", newProductWarehouseDto.productId);
        command.Parameters.AddWithValue("idOrder", newProductWarehouseDto.orderId);
        command.Parameters.AddWithValue("amount", newProductWarehouseDto.amount);
        command.Parameters.AddWithValue("price", newProductWarehouseDto.price);
        command.Parameters.AddWithValue("createdAt", newProductWarehouseDto.createdAt);
        
        await connection.OpenAsync();
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction) transaction;
        try
        {
            await command.ExecuteNonQueryAsync();
            command.Parameters.Clear();
            command.CommandText = "SELECT @@IDENTITY"; // returns last inserted id
            var id = (int) (decimal) await command.ExecuteScalarAsync();
            command.Parameters.Clear();
            command.CommandText = $"UPDATE \"Order\" SET FulfilledAt = @fullfilledAt WHERE IdOrder = @orderId";
            command.Parameters.AddWithValue("fullfilledAt", newProductWarehouseDto.createdAt);
            command.Parameters.AddWithValue("orderId", newProductWarehouseDto.orderId);
            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            return id;
        } catch (SqlException e)
        {
            await transaction.RollbackAsync();
            return null;
        } catch (Exception e)
        {
            await transaction.RollbackAsync();
            return null;
        }
    }
    
    public async Task UpdateFulfillmentDateInOrderAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        
        command.Connection = connection;
        
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}