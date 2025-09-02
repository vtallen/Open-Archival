using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactGroupingProvider : IArtifactGroupingProvider
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ArtifactGroupingProvider> _logger;

    [SetsRequiredMembers]
    public ArtifactGroupingProvider(ApplicationDbContext context, ILogger<ArtifactGroupingProvider> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(int id)
    {
        return await _context.ArtifactGroupings
            .Where(g => g.Id == id)
            .Include(g => g.Category)
            .FirstOrDefaultAsync();
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(string artifactGroupingIdentifier)
    {
        return await _context.ArtifactGroupings
            .Where(g => g.ArtifactGroupingIdentifier == artifactGroupingIdentifier)
            .Include(g => g.Category)
            .FirstOrDefaultAsync();
    }

    public async Task CreateGroupingAsync(ArtifactGrouping grouping)
    {
        _context.ArtifactGroupings.Add(grouping);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGroupingAsync(ArtifactGrouping grouping)
    {
        _context.ArtifactGroupings.Update(grouping);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGroupingAsync(int id)
    {
        await _context.ArtifactGroupings
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteGroupingAsync(ArtifactGrouping grouping)
    {
        _context.ArtifactGroupings.Remove(grouping);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<ArtifactGrouping>> GetGroupingsPaged(int pageNumber, int resultsCount)
    {
        if (pageNumber < 1 || resultsCount < 1)
        {
            throw new ArgumentOutOfRangeException($"Either page number or number of results was less than or equal to 0. {nameof(pageNumber)}={pageNumber} {nameof(resultsCount)}={resultsCount}");
        }

        var totalCount = await _context.ArtifactGroupings.CountAsync();

        var items = await _context.ArtifactGroupings
            .Include(g => g.Category)
            .OrderBy(g => g.Id)
            .Skip((pageNumber - 1) * resultsCount)
            .Take(resultsCount)
            .ToListAsync();
        
        return items;
    }
}