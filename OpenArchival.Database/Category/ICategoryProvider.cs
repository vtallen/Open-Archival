using OpenArchival.Core;

namespace OpenArchival.Database;

public interface ICategoryProvider
{
    public Task<Category?> GetCategoryAsync(string categoryName);

    public Task<int?> GetCategoryId(string categoryName);

    public Task<IEnumerable<Category>> TopCategories(int numResults);

    public Task<IEnumerable<Category>> SearchCategories(string searchQuery);

    public Task<int> InsertCategoryAsync(Category category);

    public Task<int> UpdateCategoryAsync(string categoryName, Category category);

    public Task<IEnumerable<Category>> AllCategories();

    public Task<int> InsertCategoryFieldOptionAsync(CategoryFieldOption option);

    public Task<IEnumerable<CategoryFieldOption>> GetCategoryFieldOptionsAsync(int categoryId, int fieldNumber);
}
