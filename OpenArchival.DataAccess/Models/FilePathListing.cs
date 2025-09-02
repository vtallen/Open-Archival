using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenArchival.DataAccess;

public class FilePathListing
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public ArtifactEntry? ParentArtifactEntry { get; set; }

    public required string OriginalName { get; set; }

    public required string Path { get; set; }
}
