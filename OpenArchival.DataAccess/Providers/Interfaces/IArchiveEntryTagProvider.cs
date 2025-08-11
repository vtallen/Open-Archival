namespace OpenArchival.DataAccess;

public interface IArchiveEntryTagProvider
{
    public Task<ArtifactEntryTag?> GetEntryTagAsync(int id);

    public Task<List<ArtifactEntryTag>?> GetEntryTagAsync(string name);

    public Task UpdateEntryTagAsync(ArtifactEntryTag entryTag);

    public Task CreateEntryTagAsync(ArtifactEntryTag entryTag);

    public Task DeleteEntryTagAsync(ArtifactEntryTag entryTag);

    public Task<List<ArtifactEntryTag>?> Search(string query);

    public Task<List<ArtifactEntryTag>?> Top(int count);
}
