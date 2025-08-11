namespace OpenArchival.DataAccess;

public interface IArtifactTypeProvider
{
    Task CreateArtifactTypeAsync(ArtifactType artifactType);
    Task UpdateArtifactTypeAsync(ArtifactType artifactType);
    Task DeleteArtifactTypeAsync(ArtifactType artifactType);
    Task<List<ArtifactType>?> GetArtifactType(string name);
    Task<ArtifactType?> GetArtifactType(int id);
    Task<List<ArtifactType>?> GetAllArtifactTypes();
    Task<List<ArtifactType>?> Search(string query);
    Task<List<ArtifactType>?> Top(int count);
}