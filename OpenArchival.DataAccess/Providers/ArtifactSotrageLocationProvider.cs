using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;

public class ArtifactStorageLocationProvider : IArtifactStorageLocationProvider
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly ILogger _logger;

    [SetsRequiredMembers]
    public ArtifactStorageLocationProvider(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ArtifactStorageLocationProvider> logger)
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

        // STEP 1: Get the unique location STRINGS that match the search query.
        // This simple query is guaranteed to be translated correctly by EF Core.
        var uniqueMatchingNames = await context.ArtifactStorageLocations
            .Where(p => p.Location.ToLower().Contains(query.ToLower()))
            .Select(p => p.Location)
            .Distinct()
            .ToListAsync();

        // If no names matched, return an empty list immediately.
        if (uniqueMatchingNames is null || !uniqueMatchingNames.Any())
        {
            return new List<ArtifactStorageLocation>();
        }

        // STEP 2: Now, fetch the full objects that correspond to the unique names.
        var matchingLocations = await context.ArtifactStorageLocations
            .Where(p => uniqueMatchingNames.Contains(p.Location))
            .ToListAsync();

        // STEP 3: Perform a final DistinctBy on the small in-memory list to ensure
        // a clean result set with one object per location name.
        var finalResults = matchingLocations
            .DistinctBy(p => p.Location)
            .OrderBy(p => p.Location) // Optional: Orders the final search results
            .ToList();

        return finalResults;
    }

    public async Task<List<ArtifactStorageLocation>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        // STEP 1: Get a unique, ordered list of the TOP N location *strings*.
        // This is a simple query that EF Core can always translate correctly.
        var uniqueLocationNames = await context.ArtifactStorageLocations
            .Select(p => p.Location)
            .Distinct()
            .OrderBy(locationName => locationName)
            .Take(count)
            .ToListAsync();

        if (uniqueLocationNames is null || !uniqueLocationNames.Any())
        {
            return new List<ArtifactStorageLocation>();
        }

        // STEP 2: Fetch all the full ArtifactStorageLocation objects that match the unique names.
        // We use the list from Step 1 to create a 'WHERE IN (...)' clause.
        var matchingLocations = await context.ArtifactStorageLocations
            .Where(p => uniqueLocationNames.Contains(p.Location))
            .ToListAsync();

        // STEP 3: The previous query might fetch duplicates (e.g., two entries for "Box A").
        // We now perform the final DistinctBy in-memory, which is guaranteed to work.
        var finalResults = matchingLocations
            .DistinctBy(p => p.Location)
            .OrderBy(p => p.Location) // Re-apply ordering to the final list
            .ToList();

        return finalResults;
    }
}