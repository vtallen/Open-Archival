using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class FilePathListingProvider : IFilePathListingProvider
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FilePathListingProvider> _logger;

    [SetsRequiredMembers]
    public FilePathListingProvider(ApplicationDbContext context, ILogger<FilePathListingProvider> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<FilePathListing?> GetFilePathListingAsync(int id)
    {
        return await _context.ArtifactFilePaths.Where(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task<FilePathListing?> GetFilePathListingByPathAsync(string path)
    {
        return await _context.ArtifactFilePaths.Where(f => f.Path == path).FirstOrDefaultAsync();
    }

    public async Task CreateFilePathListingAsync(FilePathListing filePathListing)
    {
        _context.ArtifactFilePaths.Add(filePathListing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFilePathListingAsync(FilePathListing filePathListing)
    {
        _context.ArtifactFilePaths.Update(filePathListing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilePathListingAsync(FilePathListing filePathListing)
    {
        _context.ArtifactFilePaths.Remove(filePathListing);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteFilePathListingAsync(string originalFileName, string diskPath)
    {
        var listingToDelete = await _context.ArtifactFilePaths
            .Where(p => p.OriginalName == originalFileName)
            .Where(p => p.Path == diskPath)
            .FirstOrDefaultAsync();

        if (listingToDelete == null)
        {
            return false;
        }

        _context.RemoveRange(listingToDelete);
        return true;
    }
}