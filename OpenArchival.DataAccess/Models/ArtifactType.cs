using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class ArtifactType
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }
}
