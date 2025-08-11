using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class FilePathListing
{
    [Key]
    public int Id { get; set; }

    public ArtifactEntry? ParentArtifactEntry { get; set; }

    public int? ParentArtifactEntryId { get; set; }

    public required string OriginalName { get; set; }

    public required string Path { get; set; }
}
