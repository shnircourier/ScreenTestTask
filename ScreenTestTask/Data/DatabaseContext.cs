using System.Data.SqlClient;
using Dapper;

namespace ScreenTestTask.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertData<T>(string sql, T entities)
    {
        await using var connection = new SqlConnection(_connectionString);
        
        try
        {
            await connection.OpenAsync();

            var id = await connection.QuerySingleAsync<int>(sql, entities);

            await connection.CloseAsync();
            
            Console.WriteLine($"Cущность {typeof(T)} c идентификатором {id} добавлена");

            return id;
        }
        catch (Exception e)
        {
            await connection.CloseAsync();
            Console.WriteLine($"Ошибка записи {typeof(T)}");
            throw new ApplicationException(e.Message);
        }
    }
}