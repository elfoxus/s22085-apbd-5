using System.Data.SqlClient;
using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public class OrderRepository : IOrderRepository
{
    
    private readonly string _connectionString;
    
    public OrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");
    }
    
    public async Task<OrderDto?> FindOrderByProductIdAndAmountAndDateBeforeAsync(FindOrderDto findOrderDto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand();
        
        command.Connection = connection;
        // command.CommandText = $"SELECT * FROM \"Order\" WHERE IdProduct = {findOrderDto.productId} AND AmountBefore = {findOrderDto.amount} AND CreatedAt < {findOrderDto.createdAt} AND FulfilledAt IS NULL";
        command.CommandText = "SELECT * FROM \"Order\" WHERE IdProduct = @productId AND Amount = @amount AND CreatedAt < @createdAt AND FulfilledAt IS NULL";
        command.Parameters.AddWithValue("productId", findOrderDto.productId);
        command.Parameters.AddWithValue("amount", findOrderDto.amount);
        command.Parameters.AddWithValue("createdAt", findOrderDto.createdAt);
            
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
            
        await reader.ReadAsync();
        var order = new OrderDto()
        {
            OrderId = (int)reader["IdOrder"],
            ProductId = (int) reader["IdProduct"],
            Amount = (int) reader["Amount"],
            CreatedAt = (DateTime) reader["CreatedAt"],
            FulfilledAt = reader["FulfilledAt"] as DateTime?
        };
            
        return order;
    }
}