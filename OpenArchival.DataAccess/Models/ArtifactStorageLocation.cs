using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class ArtifactStorageLocation
{
    [Key]
    public int Id { get; set; }

    public required string Location { get; set; }
}
