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
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.StorageLocation)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Files)
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(string artifactGroupingIdentifier)
    {
        await using var context = await _context.CreateDbContextAsync();
        return await context.ArtifactGroupings
            .Include(g => g.Category)
            .Include(g => g.IdentifierFields)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.StorageLocation)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Type)
            .Include(g => g.ChildArtifactEntries)
                .ThenInclude(e => e.Files)
            .Where(g => g.ArtifactGroupingIdentifier == artifactGroupingIdentifier)
            .FirstOrDefaultAsync();
    }

    public async Task CreateGroupingAsync(ArtifactGrouping grouping)
    {
        await using var context = await _context.CreateDbContextAsync();
        context.ArtifactGroupings.Add(grouping);
        await context.SaveChangesAsync();
    }

    public async Task UpdateGroupingAsync(ArtifactGrouping grouping)
    {
        await using var context = await _context.CreateDbContextAsync();
        context.ArtifactGroupings.Update(grouping);
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