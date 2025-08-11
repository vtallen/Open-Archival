using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ListedNameProvider : IListedNameProvider
{
    private readonly IDbContextFactory<ArchiveDbContext> _dbFactory;
    private readonly ILogger<ListedNameProvider> _logger;

    [SetsRequiredMembers]
    public ListedNameProvider(IDbContextFactory<ArchiveDbContext> context, ILogger<ListedNameProvider> logger)
    {
        _dbFactory = context;
        _logger = logger;
    }

    public async Task<ListedName?> GetAssociatedNameAsync(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactAssociatedNames.Where(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ListedName>?> GetAssociatedNamesAsync(string firstName, string lastName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactAssociatedNames
            .Where(n => n.FirstName == firstName && n.LastName == lastName)
            .ToListAsync();
    }

    public async Task CreateAssociatedNameAsync(ListedName associatedName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactAssociatedNames.Add(associatedName);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAssociatedNameAsync(ListedName associatedName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactAssociatedNames.Update(associatedName); 
        await context.SaveChangesAsync(); 
    }

    public async Task DeleteAssociatedNameAsync(ListedName associatedName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArtifactAssociatedNames.Remove(associatedName);

        await context.SaveChangesAsync();
    }

    public async Task<List<ListedName>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();
        var lowerCaseQuery = query.ToLower();

        return await context.ArtifactAssociatedNames
            .Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(lowerCaseQuery))
            .ToListAsync();
    }

    public async Task<List<ListedName>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArtifactAssociatedNames
            .OrderBy(p => p.FirstName)
            .Take(count)
            .ToListAsync();
    }
}