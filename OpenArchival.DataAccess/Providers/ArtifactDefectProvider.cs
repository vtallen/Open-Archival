using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactDefectProvider : IArtifactDefectProvider
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly ILogger<ArtifactDefectProvider> _logger;

    [SetsRequiredMembers]
    public ArtifactDefectProvider(IDbContextFactory<ApplicationDbContext> context, ILogger<ArtifactDefectProvider> logger)
    {
        _dbFactory = context;
        _logger = logger;
    }

    public async Task<ArtifactDefect?> GetDefectAsync(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactDefects.Where(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ArtifactDefect>?> GetDefectAsync(string description)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactDefects.Where(d => d.Description == description).ToListAsync();
    }

    public async Task UpdateDefectAsync(ArtifactDefect artifactDefect)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        context.ArtifactDefects.Update(artifactDefect);

        await context.SaveChangesAsync();
    }

    public async Task CreateDefectAsync(ArtifactDefect artifactDefect)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactDefects.Add(artifactDefect);
        await context.SaveChangesAsync();
    }

    public async Task DeleteDefectAsync(ArtifactDefect artifactDefect)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactDefects.Remove(artifactDefect);
        await context.SaveChangesAsync();
    }

    public async Task<List<ArtifactDefect>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactDefects
            .Where(p => p.Description.ToLower().Contains(query.ToLower())).ToListAsync();
    }

    public async Task<List<ArtifactDefect>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactDefects
            .OrderBy(p => p.Description)
            .Take(count)
            .ToListAsync();
    }
}