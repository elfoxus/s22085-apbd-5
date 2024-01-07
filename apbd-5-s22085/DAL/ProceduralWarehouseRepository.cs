using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using apbd_5_s22085.DTOs;

namespace apbd_5_s22085.DAL;

public class ProceduralWarehouseRepository : IProceduralWarehouseRepository
{

    private readonly string _connectionString;
    
    public ProceduralWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentException("Connection string not found");
    }
    
    public async Task<int> InsertProductWarehouse(ProductInWarehouseRequestDto dto)
    {
        await using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = CommandType.StoredProcedure,
        };
        command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", dto.Amount);
        command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
        await connection.OpenAsync();
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction) transaction;
        await command.ExecuteNonQueryAsync();
        command.Parameters.Clear();
        command.CommandText = "SELECT @@IDENTITY"; // returns last inserted id
        command.CommandType = CommandType.Text;
        var executeScalarAsync =  await command.ExecuteScalarAsync();
        await transaction.CommitAsync();
        return (int) (decimal) executeScalarAsync;
    }
}