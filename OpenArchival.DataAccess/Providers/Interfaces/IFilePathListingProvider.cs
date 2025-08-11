namespace OpenArchival.DataAccess;

public interface IFilePathListingProvider
{
    Task<FilePathListing?> GetFilePathListingAsync(int id);
    Task<FilePathListing?> GetFilePathListingByPathAsync(string path);
    Task CreateFilePathListingAsync(FilePathListing filePathListing);
    Task UpdateFilePathListingAsync(FilePathListing filePathListing);
    Task DeleteFilePathListingAsync(FilePathListing filePathListing);
    public Task<bool> DeleteFilePathListingAsync(string originalFileName, string diskPath);
}