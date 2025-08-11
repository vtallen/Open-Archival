using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactGroupingProvider : IArtifactGroupingProvider
{
    private readonly ArchiveDbContext _context;
    private readonly ILogger<ArtifactGroupingProvider> _logger;

    [SetsRequiredMembers]
    public ArtifactGroupingProvider(ArchiveDbContext context, ILogger<ArtifactGroupingProvider> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(int id)
    {
        return await _context.ArtifactGroupings.Where(g => g.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ArtifactGrouping?> GetGroupingAsync(string artifactGroupingIdentifier)
    {
        return await _context.ArtifactGroupings.Where(g => g.ArtifactGroupingIdentifier == artifactGroupingIdentifier).FirstOrDefaultAsync();
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

    public async Task DeleteGroupingAsync(ArtifactGrouping grouping)
    {
        _context.ArtifactGroupings.Remove(grouping);
        await _context.SaveChangesAsync();
    }
}