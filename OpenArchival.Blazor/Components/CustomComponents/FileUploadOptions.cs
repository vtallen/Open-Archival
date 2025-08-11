namespace OpenArchival.Blazor.Components;

public class FileUploadOptions
{
    public static string Key = "FileUploadOptions";
    public required long MaxUploadSizeBytes { get; set; }
    public required string UploadFolderPath { get; set; }

    public required int MaxFileCount { get; set; }
}
