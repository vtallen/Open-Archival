using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class ArtifactAssociatedNamesProvider : IArtifactAssociatedNamesProvider
{
    private NpgsqlDataSource _dataSource;

    public ArtifactAssociatedNamesProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> ContainsName(string name)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT EXISTS(SELECT 1 FROM artifactassociatednames WHERE name = @Name)";

        return await connection.ExecuteScalarAsync<bool>(sql, new { Name = name });
    }

    public async Task<bool> InsertName(string name)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO artifactassociatednames (name) VALUES (@Name)";

        int rowsAffected = await connection.ExecuteAsync(sql, new {Name=name});

        return rowsAffected == 1;
    }
    
    public async Task<bool> RemoveName(string name)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM artifactassociatednames WHERE name = @Name";

        int rowsAffected = await connection.ExecuteAsync(sql, new { Name = name });

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<string>> SearchNames(string query)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT name FROM artifactassociatednames WHERE POSITION(LOWER(@Query) in LOWER(name)) > 0";

        return await connection.QueryAsync<string>(sql, new { Query = query });
    }

    public async Task<IEnumerable<string>> TopNames(int resultCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT name FROM artifactassociatednames ORDER BY name ASC LIMIT {resultCount}";

        return await connection.QueryAsync<string>(sql);
    }

}
