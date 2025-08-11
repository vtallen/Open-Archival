namespace OpenArchival.DataAccess;

public interface IArchiveCategoryProvider
{
    public Task CreateCategoryAsync(ArchiveCategory category);

    public Task UpdateCategoryAsync(ArchiveCategory category);

    public Task DeleteCategoryAsync(ArchiveCategory category);

    public Task<ArchiveCategory?> GetArchiveCategory(int id);


    public Task<List<ArchiveCategory>?> GetArchiveCategory(string categoryName);

    public Task<List<ArchiveCategory>?> GetAllArchiveCategories();

    public Task<List<ArchiveCategory>?> Search(string query);

    public Task<List<ArchiveCategory>?> Top(int count);
}
