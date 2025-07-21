using Microsoft.Extensions.Options;
using Npgsql;
using Dapper;

namespace OpenArchival.Database.Category;

public class Category
{
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string FieldSeparator { get; set; }
    public required string[] FieldNames { get; set; }
    public required string[] FieldDescriptions { get; set; }
}

public class CategoryFieldOption
{
    public required int CategoryId { get; set; }
    public required int FieldNumber { get; set; }
    public required string Value { get; set; } 
    public required string Name { get; set; }
    public string? Description { get; set; }

}

public class CategoryProvider : ICategoryProvider
{
    public static string TableCreationQuery = """
        DROP TABLE IF EXISTS Categories CASCADE;
        CREATE TABLE IF NOT EXISTS Categories (
            categoryid SERIAL PRIMARY KEY, 
            categoryname TEXT NOT NULL,
            fieldseparator TEXT NOT NULL,
            fieldnames TEXT [] NOT NULL,
            fielddescriptions TEXT [] NOT NULL
        );

        CREATE TABLE IF NOT EXISTS CategoryFieldOptions (
            categoryid INT NOT NULL,
            fieldnumber INT NOT NULL,
            value TEXT NOT NULL,
            name TEXT NOT NULL,
            FOREIGN KEY (categoryid) REFERENCES Categories(categoryid)
        );
        """;
    
    private readonly PostgresConnectionOptions _options;

    private readonly NpgsqlDataSource _dataSource;

    public CategoryProvider(IOptions<PostgresConnectionOptions> options, NpgsqlDataSource databaseConnection)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options), "Postgres connection options cannot be null.");
        _dataSource = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection), "Database connection cannot be null.");

        if (_options.Host == null || _options.Port <= 0 || _options.Database == null || _options.Username == null || _options.Password == null)
        {
            throw new ArgumentException("Postgres connection options are not properly configured.");
        }
    }
    
    public async Task<Category?> GetCategoryAsync(string categoryName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"SELECT * FROM Categories WHERE CategoryName = @CategoryName";

        return await connection.QueryFirstOrDefaultAsync<Category>(sql, new {CategoryName=categoryName});
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

    public async Task<int> InsertCategoryAsync(Category category)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var sql = @"INSERT INTO Categories (categoryname, fieldseparator, fieldnames, fielddescriptions) VALUES (@CategoryName, @FieldSeparator, @FieldNames, @FieldDescriptions)";
    
        return await connection.ExecuteAsync(sql, category);
    }

    public async Task<int> UpdateCategoryAsync(string oldCategoryName, Category category)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        // 1. Add commas between each SET assignment.
        // 2. Use a distinct parameter (e.g., @OldCategoryName) in the WHERE clause.
        const string sql = @"
        UPDATE Categories
        SET
            categoryname = @CategoryName,
            fieldseparator = @FieldSeparator,
            fieldnames = @FieldNames,
            fielddescriptions = @FieldDescriptions
        WHERE categoryname = @OldCategoryName;";

        // 3. Create a parameter object that includes the value for the WHERE clause.
        var parameters = new
        {
            // These parameters come from the 'category' object for the SET clause
            category.CategoryName,
            category.FieldSeparator,
            category.FieldNames,
            category.FieldDescriptions,

            // This parameter comes from the method argument for the WHERE clause
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
