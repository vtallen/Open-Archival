namespace OpenArchival.Database.Category;

public interface ICategoryProvider
{
    public Task<Category?> GetCategoryAsync(string categoryName);

    public Task<int?> GetCategoryId(string categoryName);

    public Task<int> InsertCategoryAsync(Category category);

    public Task<int> UpdateCategoryAsync(string categoryName, Category category);

    public Task<IEnumerable<Category>> AllCategories();

    public Task<int> InsertCategoryFieldOptionAsync(CategoryFieldOption option);

    public Task<IEnumerable<CategoryFieldOption>> GetCategoryFieldOptionsAsync(int categoryId, int fieldNumber);
}
