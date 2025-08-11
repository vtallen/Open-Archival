namespace OpenArchival.DataAccess;

public interface IArtifactStorageLocationProvider
{
    Task CreateArtifactStorageLocationAsync(ArtifactStorageLocation location);
    Task UpdateArtifactStorageLocationAsync(ArtifactStorageLocation location);
    Task DeleteArtifactStorageLocationAsync(ArtifactStorageLocation location);
    Task<List<ArtifactStorageLocation>?> GetArtifactStorageLocation(string locationName);
    Task<ArtifactStorageLocation?> GetArtifactStorageLocation(int id);
    Task<List<ArtifactStorageLocation>?> GetAllArtifactStorageLocations();
    Task<List<ArtifactStorageLocation>?> Search(string query);
    Task<List<ArtifactStorageLocation>?> Top(int count);
}