namespace OpenArchival.DataAccess;

public interface IArtifactDefectProvider
{
    public Task<ArtifactDefect?> GetDefectAsync(int id);
    public Task<List<ArtifactDefect>?> GetDefectAsync(string description);
    public Task UpdateDefectAsync(ArtifactDefect artifactDefect);
    public Task CreateDefectAsync(ArtifactDefect artifactDefect);
    public Task DeleteDefectAsync(ArtifactDefect artifactDefect);
    public Task<List<ArtifactDefect>?> Search(string query);
    public Task<List<ArtifactDefect>?> Top(int count);
}