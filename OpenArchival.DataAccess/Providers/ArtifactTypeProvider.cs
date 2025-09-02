using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactTypeProvider : IArtifactTypeProvider
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly ILogger<ArtifactTypeProvider> _logger;

    [SetsRequiredMembers]
    public ArtifactTypeProvider(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ArtifactTypeProvider> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    public async Task CreateArtifactTypeAsync(ArtifactType artifactType)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        context.ArtifactTypes.Add(artifactType);
        await context.SaveChangesAsync();
    }

    public async Task UpdateArtifactTypeAsync(ArtifactType artifactType)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        context.ArtifactTypes.Update(artifactType);
        await context.SaveChangesAsync();
    }

    public async Task DeleteArtifactTypeAsync(ArtifactType artifactType)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        context.ArtifactTypes.Remove(artifactType);
        await context.SaveChangesAsync();
    }

    public async Task<List<ArtifactType>?> GetArtifactType(string name)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactTypes.Where(a => a.Name == name).ToListAsync();
    }

    public async Task<ArtifactType?> GetArtifactType(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactTypes.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<ArtifactType>?> GetAllArtifactTypes()
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactTypes.ToListAsync();
    }

    public async Task<List<ArtifactType>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactTypes
            .Where(p => p.Name.ToLower().Contains(query.ToLower()))
            .ToListAsync();
    }

    public async Task<List<ArtifactType>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        return await context.ArtifactTypes
            .OrderBy(p => p.Name)
            .Take(count)
            .ToListAsync();
    }
}