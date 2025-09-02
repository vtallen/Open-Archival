using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArchiveCategoryProvider : IArchiveCategoryProvider
{
    private Microsoft.EntityFrameworkCore.IDbContextFactory<ApplicationDbContext> _dbFactory;
    private ILogger _logger;

    [SetsRequiredMembers]
    public ArchiveCategoryProvider(Microsoft.EntityFrameworkCore.IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<ArchiveCategoryProvider> logger) 
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }
    
    public async Task CreateCategoryAsync(ArchiveCategory category)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArchiveCategories.Add(category);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(ArchiveCategory category)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArchiveCategories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(ArchiveCategory category)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        context.ArchiveCategories.Remove(category);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<ArchiveCategory>?> GetArchiveCategory(string categoryName)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArchiveCategories.Where(a => a.Name == categoryName).ToListAsync();
    }

    public async Task<ArchiveCategory?> GetArchiveCategory(int id)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArchiveCategories.Where(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ArchiveCategory>?> GetAllArchiveCategories()
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArchiveCategories.ToListAsync();
    }

    public async Task<List<ArchiveCategory>?> Search(string query)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArchiveCategories
            .Where(p => p.Name.ToLower().Contains(query.ToLower())).ToListAsync();
    }

    public async Task<List<ArchiveCategory>?> Top(int count)
    {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.ArchiveCategories
            .OrderBy(p => p.Name)
            .Take(count)
            .ToListAsync();
    }
}
