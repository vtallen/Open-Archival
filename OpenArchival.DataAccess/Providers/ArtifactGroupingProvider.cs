using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactGroupingProvider : IArtifactGroupingProvider
{
    private readonly IDbContextFactory<ApplicationDbContext>  _context;
    private readonly ILogger<ArtifactGroupingProvider> _logger;

    [SetsRequiredMembers]
    public ArtifactGroupingProvider(IDbContextFactory<ApplicationDbContext> context, ILogger<ArtifactGroupingProvider> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(int id)
    {
        await using var context = await _context.CreateDbContextAsync();
        return await context.ArtifactGroupings
            .Include(g => g.Category)
            .Include(g => g.IdentifierFields)
            .Include(g => g.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.StorageLocation)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Files)
            .Include(g=> g.ChildArtifactEntries)
                .ThenInclude(e => e.Tags)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.ListedNames)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Defects)
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(string artifactGroupingIdentifier)
    {
        await using var context = await _context.CreateDbContextAsync();
        return await context.ArtifactGroupings
            .Include(g => g.Category)
            .Include(g => g.IdentifierFields)
            .Include(g => g.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.StorageLocation)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Files)
            .Include(g=> g.ChildArtifactEntries)
                .ThenInclude(e => e.Tags)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.ListedNames)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Defects)
            .Where(g => g.ArtifactGroupingIdentifier == artifactGroupingIdentifier)
            .FirstOrDefaultAsync();
    }

    public async Task CreateGroupingAsync(ArtifactGrouping grouping)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Iterate through all child entries and their file paths.
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            // Create a temporary list to hold the managed file path entities.
            var managedFilePaths = new List<FilePathListing>();

            // Handle each file path in the entry.
            foreach (var filepath in entry.Files)
            {
                // Attempt to find the file path in the database.
                var existingFilePath = await context.ArtifactFilePaths.FirstOrDefaultAsync(f => f.Path == filepath.Path);

                if (existingFilePath != null)
                {
                    // The file path already exists. Use the tracked instance.
                    managedFilePaths.Add(existingFilePath);
                }
                else
                {
                    // The file path is new. Add it to the managed list.
                    managedFilePaths.Add(filepath);
                }
            }

            // Replace the disconnected file path objects on the entry with the managed ones.
            entry.Files = managedFilePaths;
        }

        context.ArtifactGroupings.Add(grouping);
        await context.SaveChangesAsync();
    }

    public async Task UpdateGroupingAsync(ArtifactGrouping grouping)
    {
        await using var context = await _context.CreateDbContextAsync();

        // **NEW LOGIC**
        // Fetch the existing, tracked entity and its entire graph.
        var existingGrouping = await context.ArtifactGroupings
            .Include(g => g.Category)
            .Include(g => g.Type)
            .Include(g => g.IdentifierFields)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.StorageLocation)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Files)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Tags)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.ListedNames)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Defects)
            .FirstOrDefaultAsync(g => g.Id == grouping.Id);

        if (existingGrouping == null)
        {
            throw new InvalidOperationException($"Grouping with ID {grouping.Id} not found for update.");
        }

        // Update top-level properties.
        existingGrouping.Title = grouping.Title;
        existingGrouping.IsPublicallyVisible = grouping.IsPublicallyVisible;
        existingGrouping.Description = grouping.Description;

        // Manually manage collections to sync the in-memory graph.
        var entriesInModel = grouping.ChildArtifactEntries.ToDictionary(e => e.Id);

        // Remove entries from the database that are no longer in the model.
        existingGrouping.ChildArtifactEntries.RemoveAll(e => !entriesInModel.ContainsKey(e.Id));

        // Add or update existing entries.
        foreach (var entryInModel in grouping.ChildArtifactEntries)
        {
            var existingEntry = existingGrouping.ChildArtifactEntries.FirstOrDefault(e => e.Id == entryInModel.Id);

            if (existingEntry != null)
            {
                // Update an EXISTING entry.
                existingEntry.Title = entryInModel.Title;
                // Sync the files collection.
                var filesInModel = entryInModel.Files.Select(f => f.Path).ToHashSet();
                existingEntry.Files.RemoveAll(f => !filesInModel.Contains(f.Path));

                var newFilesToRelate = filesInModel.Except(existingEntry.Files.Select(f => f.Path));
                foreach (var filePath in newFilesToRelate)
                {
                    var fileToAdd = await context.ArtifactFilePaths.FirstOrDefaultAsync(f => f.Path == filePath);
                    if (fileToAdd != null)
                    {
                        existingEntry.Files.Add(fileToAdd);
                    }
                }
            }
            else
            {
                // Add a NEW entry to the existing grouping.
                var newEntry = entryInModel;

                // For this new entry, handle the file path relationships.
                var existingFiles = await context.ArtifactFilePaths
                    .Where(fp => newEntry.Files.Select(f => f.Path).Contains(fp.Path))
                    .ToListAsync();
                newEntry.Files.Clear();
                foreach (var existingFile in existingFiles)
                {
                    newEntry.Files.Add(existingFile);
                }

                existingGrouping.ChildArtifactEntries.Add(newEntry);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteGroupingAsync(int id)
    {
        await using var context = await _context.CreateDbContextAsync();
        await context.ArtifactGroupings
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();
    }

    public async Task DeleteGroupingAsync(ArtifactGrouping grouping)
    {
        await using var context = await _context.CreateDbContextAsync();
        context.ArtifactGroupings.Remove(grouping);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<ArtifactGrouping>> GetGroupingsPaged(int pageNumber, int resultsCount)
    {
        await using var context = await _context.CreateDbContextAsync();
        if (pageNumber < 1 || resultsCount < 1)
        {
            throw new ArgumentOutOfRangeException($"Either page number or number of results was less than or equal to 0. {nameof(pageNumber)}={pageNumber} {nameof(resultsCount)}={resultsCount}");
        }

        var totalCount = await context.ArtifactGroupings.CountAsync();

        var items = await context.ArtifactGroupings
            .Include(g => g.ChildArtifactEntries)
            .Include(g => g.Category)
            .OrderBy(g => g.Id)
            .Skip((pageNumber - 1) * resultsCount)
            .Take(resultsCount)
            .ToListAsync();
        
        return items;
    }

    public async Task<int> GetTotalCount()
    {
        await using var context = await _context.CreateDbContextAsync();
        return context.ArtifactGroupings.Count();
    }
}