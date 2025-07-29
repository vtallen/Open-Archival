using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class FilePathProvider : IFilePathProvider
{
    NpgsqlDataSource _dataSource;

    public FilePathProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> AddFile(string path)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO archivefiles (filename, path) VALUES (@Filename, @Path)";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Filename = Path.GetFileName(path), Path = path});

        return rowsAffected == 1;
    }

    public async Task<bool> RemoveFile(string path)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM archivefiles WHERE path = @Path";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Path = path });

        return rowsAffected == 1;
    }

    public async Task<IEnumerable<FileInfo>> SearchFiles(string filename)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM archivefiles WHERE POSITION(LOWER(@Query) in LOWER(filename)) > 0";

        return await connection.QueryAsync<FileInfo>(sql, new { Query = filename });
    }

    public async Task<IEnumerable<FileInfo>> TopFiles(int resultsCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT * FROM archivefiles ORDER BY filename ASC LIMIT {resultsCount}";

        return await connection.QueryAsync<FileInfo>(sql);
    }
}
