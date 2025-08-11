using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

    public async Task<bool> AddFileInfo(string path, string originalName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO archivefiles (originalname, path) VALUES (@OriginalName, @Path)";

        var rowsAffected = await connection.ExecuteAsync(sql, new { OriginalName=originalName, Path = path});

        return rowsAffected == 1;
    }

    public async Task<bool> RemoveFileInfo(string path, string originalName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM archivefiles WHERE path = @Path AND originalname = @OriginalName";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Path = path, OriginalName = originalName});

        return rowsAffected == 1;
    }

    public async Task<IEnumerable<FilePathInfo>> SearchFiles(string filename)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM archivefiles WHERE POSITION(LOWER(@Query) in LOWER(filename)) > 0";

        return await connection.QueryAsync<FilePathInfo>(sql, new { Query = filename });
    }

    public async Task<IEnumerable<FilePathInfo>> TopFiles(int resultsCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT * FROM archivefiles ORDER BY filename ASC LIMIT {resultsCount}";

        return await connection.QueryAsync<FilePathInfo>(sql);
    }
}
