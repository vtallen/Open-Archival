using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class DefectsProvider : IDefectsProvider
{
    NpgsqlDataSource _dataSource;

    public DefectsProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> AddDefect(string defect)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO defects (defect) VALUES (@Defect)";

        var rowsAffected = await connection.ExecuteAsync(sql, new {Defect=defect});

        return rowsAffected == 1;
    }

    public async Task<bool> RemoveDefect(string defect)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM defects WHERE defect = @Defect";

        var rowsAffected = await connection.ExecuteAsync(sql, new {Defect=defect});

        return rowsAffected == 1;
    }

    public async Task<bool> ContainsDefect(string defect)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT EXISTS(SELECT 1 FROM defects WHERE defect = @Defect)";

        return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { Defect = defect });
    }

    public async Task<IEnumerable<string>> SearchDefects(string query)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT defect FROM defects WHERE POSITION(LOWER(@Query) in LOWER(defect)) > 0";

        return await connection.QueryAsync<string>(sql, new { Query=query });
    }

    public async Task<IEnumerable<string>> TopDefects(int resultCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT defect FROM defects ORDER BY defect ASC LIMIT {resultCount}";

        return await connection.QueryAsync<string>(sql);
    }
}
