using Microsoft.Extensions.Options;
using Npgsql;
using Dapper;
using OpenArchival.Core;

namespace OpenArchival.Database;

public class CategoryProvider : ICategoryProvider
{
    private readonly NpgsqlDataSource _dataSource;

    public CategoryProvider(NpgsqlDataSource databaseConnection)
    {
        _dataSource = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection), "Database connection cannot be null.");
    }
    
    public async Task<Category?> GetCategoryAsync(string categoryName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM Categories WHERE CategoryName = @CategoryName";

        return await connection.QueryFirstOrDefaultAsync<Category>(sql, new {CategoryName=categoryName});
    }

    public async Task<bool> RemoveCategory(string categoryName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"DELETE FROM Categories WHERE categoryname = @CategoryName";

        int rowsAffected = await connection.ExecuteAsync(sql, new { CategoryName = categoryName });

        return rowsAffected > 0;
    }

    public async Task<int?> GetCategoryId(string categoryName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT categoryid FROM Categories WHERE categoryname = @CategoryName";

        return await connection.QueryFirstOrDefaultAsync<int>(sql, new {CategoryName=categoryName});
    }

    public async Task<IEnumerable<Category>> AllCategories()
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM Categories;";

        return await connection.QueryAsync<Category>(sql);
    }

    public async Task<IEnumerable<Category>> SearchCategories(string searchQuery)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM Categories WHERE POSITION(LOWER(@SearchQuery) in LOWER(categoryname)) > 0";

        return await connection.QueryAsync<Category>(sql, new {SearchQuery=searchQuery});
    }

    public async Task<IEnumerable<Category>> TopCategories(int numResults)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = $@"SELECT * FROM Categories ORDER BY categoryname ASC LIMIT {numResults}";

        return await connection.QueryAsync<Category>(sql);
    }

    public async Task<int> InsertCategoryAsync(Category category)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO Categories (categoryname, fieldseparator, fieldnames, fielddescriptions) VALUES (@CategoryName, @FieldSeparator, @FieldNames, @FieldDescriptions)";
    
        return await connection.ExecuteAsync(sql, category);
    }

    public async Task<int> UpdateCategoryAsync(string oldCategoryName, Category category)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        const string sql = @"
        UPDATE Categories
        SET
            categoryname = @CategoryName,
            fieldseparator = @FieldSeparator,
            fieldnames = @FieldNames,
            fielddescriptions = @FieldDescriptions
        WHERE categoryname = @OldCategoryName;";

        var parameters = new
        {
            category.CategoryName,
            category.FieldSeparator,
            category.FieldNames,
            category.FieldDescriptions,

            OldCategoryName = oldCategoryName
        };

        return await connection.ExecuteAsync(sql, parameters);
    }

    public async Task<int> InsertCategoryFieldOptionAsync(CategoryFieldOption option)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var sql = @"INSERT INTO CategoryFieldOptions (categoryid, fieldnumber, value, name, description) VALUES (@CategoryId, @FieldNumber, @Value, @Name, @Description)";
    
        return await connection.ExecuteAsync(sql, option);
    }

    public async Task<IEnumerable<CategoryFieldOption>> GetCategoryFieldOptionsAsync(int categoryId, int fieldNumber)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var sql = @"SELECT * FROM CategoryFieldOptions WHERE categoryid = @CategoryId AND fieldnumber = @FieldNumber";
        return await connection.QueryAsync<CategoryFieldOption>(sql, new {CategoryId = categoryId, FieldNumber = fieldNumber});
    }
}
