using Dapper;
using Npgsql;

namespace OpenArchival.Database;

public class ArchiveStorageLocationProvider : IArchiveStorageLocationProvider
{
    private NpgsqlDataSource _dataSource;

    public ArchiveStorageLocationProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> ContainsLocation(string location)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT EXISTS(SELECT 1 FROM archivestoragelocations WHERE location = @Location)";
        
        return await connection.ExecuteScalarAsync<bool>(sql, new {Location = location});
    }

    public async Task<bool> AddLocation(string location)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO archivestoragelocations (location) VALUES (@Location)";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Location = location });
        
        return rowsAffected == 1;
    }

    public async Task<bool> RemoveLocation(string location)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM archivestoragelocations WHERE location = @Location";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Location = location });

        return rowsAffected == 1;
    }

    public async Task<IEnumerable<string>> SearchLocations(string query)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT location FROM archivestoragelocations WHERE POSITION(LOWER(@Query) in LOWER(location)) > 0";

        return await connection.QueryAsync<string>(sql, new {Query=query});
    }

    public async Task<IEnumerable<string>> TopLocations(int resultCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT location FROM archivestoragelocations ORDER BY location ASC LIMIT {resultCount}";

        return await connection.QueryAsync<string>(sql);
    }
}
