using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;

public class ArtifactStorageLocationProvider : IArtifactStorageLocationProvider
{
    private readonly IDbContextFactory<ArchiveDbContext> _dbFactory;
    private readonly ILogger _logger;

    [SetsRequiredMembers]
    public ArtifactStorageLocationProvider(IDbContextFactory<ArchiveDbContext> dbFactory, ILogger<ArtifactStorageLocationProvider> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    public async Task CreateArtifactStorageLocationAsync(ArtifactStorageLocation location)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactStorageLocations.Add(location);
        await context.SaveChangesAsync();
    }

    public async Task UpdateArtifactStorageLocationAsync(ArtifactStorageLocation location)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactStorageLocations.Update(location);
        await context.SaveChangesAsync();
    }

    public async Task DeleteArtifactStorageLocationAsync(ArtifactStorageLocation location)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactStorageLocations.Remove(location);
        await context.SaveChangesAsync();
    }

    public async Task<List<ArtifactStorageLocation>?> GetArtifactStorageLocation(string locationName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactStorageLocations.Where(a => a.Location == locationName).ToListAsync();
    }

    public async Task<ArtifactStorageLocation?> GetArtifactStorageLocation(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactStorageLocations.Where(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ArtifactStorageLocation>?> GetAllArtifactStorageLocations()
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactStorageLocations.ToListAsync();
    }

    public async Task<List<ArtifactStorageLocation>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactStorageLocations
            .Where(p => p.Location.ToLower().Contains(query.ToLower())).ToListAsync();
    }

    public async Task<List<ArtifactStorageLocation>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactStorageLocations
            .OrderBy(p => p.Location)
            .Take(count)
            .ToListAsync();
    }
}