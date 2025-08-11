using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArchiveEntryTagProvider : IArchiveEntryTagProvider
{
    private readonly IDbContextFactory<ArchiveDbContext> _dbFactory;
    private readonly ILogger<ArchiveEntryTagProvider> _logger;

    [SetsRequiredMembers]
    public ArchiveEntryTagProvider(IDbContextFactory<ArchiveDbContext> context, ILogger<ArchiveEntryTagProvider> logger)
    {
        _dbFactory = context;
        _logger = logger;
    }

    public async Task<ArtifactEntryTag?> GetEntryTagAsync(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactEntryTags.Where(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ArtifactEntryTag>?> GetEntryTagAsync(string name)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactEntryTags.Where(t => t.Name == name).ToListAsync();
    }

    public async Task UpdateEntryTagAsync(ArtifactEntryTag entryTag)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactEntryTags.Update(entryTag);
        await context.SaveChangesAsync();
    }

    public async Task CreateEntryTagAsync(ArtifactEntryTag entryTag)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactEntryTags.Add(entryTag);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEntryTagAsync(ArtifactEntryTag entryTag)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactEntryTags.Remove(entryTag);
        await context.SaveChangesAsync();
    }

    public async Task<List<ArtifactEntryTag>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactEntryTags
            .Where(p => p.Name.ToLower().Contains(query.ToLower())).ToListAsync();
    }

    public async Task<List<ArtifactEntryTag>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactEntryTags
            .OrderBy(p => p.Name)
            .Take(count)
            .ToListAsync();
    }
}