using Npgsql;
using Dapper;

namespace OpenArchival.Database;

public class ArtifactTypesProvider : IArtifactTypesProvider
{
    private NpgsqlDataSource _dataSource;

    public ArtifactTypesProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> ContainsType(string type)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT EXISTS(SELECT 1 FROM artifacttypes WHERE type = @Value)";

        return await connection.ExecuteScalarAsync<bool>(sql, new { Value=type});
    } 

    public async Task<bool> AddType(string type)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO artifacttypes (type) VALUES (@Type)";

        int rowsAffected = await connection.ExecuteAsync(sql, new { Type = type });

        return rowsAffected == 1;
    }

    public async Task<bool> RemoveType(string type)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM artifacttypes WHERE value = @Value";

        int rowsAffected = await connection.ExecuteAsync(sql, new { Value = type });

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<string>> SearchTypes(string query)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT type FROM artifacttypes WHERE POSITION(LOWER(@Query) in LOWER(type)) > 0";

        return await connection.QueryAsync<string>(sql, new {Query=query});
    }

    public async Task<IEnumerable<string>> TopTypes(int resultCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @$"SELECT type FROM artifacttypes ORDER BY type ASC LIMIT {resultCount}";

        return await connection.QueryAsync<string>(sql);
    }
}
