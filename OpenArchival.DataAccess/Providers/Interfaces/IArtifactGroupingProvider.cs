namespace OpenArchival.DataAccess;

public interface IArtifactGroupingProvider
{
    Task<ArtifactGrouping?> GetGroupingAsync(int id);
    Task<ArtifactGrouping?> GetGroupingAsync(string artifactGroupingIdentifier);
    Task CreateGroupingAsync(ArtifactGrouping grouping);
    Task UpdateGroupingAsync(ArtifactGrouping grouping);
    Task DeleteGroupingAsync(int id);
    Task DeleteGroupingAsync(ArtifactGrouping grouping);
    Task<List<ArtifactGrouping>> GetGroupingsPaged(int pageNumber, int resultsCount);
    public Task<int> GetTotalCount();
}