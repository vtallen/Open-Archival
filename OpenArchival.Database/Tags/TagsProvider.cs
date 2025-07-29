using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class TagsProvider : ITagsProvider
{
    private NpgsqlDataSource _dataSource;

    public TagsProvider(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;   
    }

    public async Task<bool> AddTag(string tag)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO tags (tag) VALUES (@Tag)";
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { Tag = tag });

        return rowsAffected == 1;
    }

    public async Task<bool> RemoveTag(string tag)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM tags WHERE tag = @Tag";

        var rowsAffected = await connection.ExecuteAsync(sql, new { Tag = tag });

        return rowsAffected == 1;
    }

    public async Task<bool> ContainsTag(string tag)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT EXISTS(SELECT 1 FROM tags WHERE tag = @Tag)";

        return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { Tag = tag});
    }

    public async Task<IEnumerable<string>> TopTags(int resultCount)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT FROM tags ORDER BY tag ASC LIMIT {resultCount}";

        return await connection.QueryAsync<string>(sql);
    }

    public async Task<IEnumerable<string>> SearchTags(string query)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT tag FROM tags WHERE POSITION(LOWER(@Query) in LOWER(tag)) > 0";

        return await connection.QueryAsync<string>(sql, new { Query = query });
    }
}
