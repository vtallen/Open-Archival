namespace OpenArchival.DataAccess;

public interface IListedNameProvider
{
    Task<ListedName?> GetAssociatedNameAsync(int id);
    Task<List<ListedName>?> GetAssociatedNamesAsync(string name);
    Task CreateAssociatedNameAsync(ListedName associatedName);
    Task UpdateAssociatedNameAsync(ListedName associatedName);
    Task DeleteAssociatedNameAsync(ListedName associatedName);
    public Task<List<ListedName>?> Search(string query);
    public Task<List<ListedName>?> Top(int count);
}