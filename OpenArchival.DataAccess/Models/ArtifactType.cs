using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenArchival.DataAccess;

public class ArtifactType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique =true)]
    public required string Name { get; set; }

    public List<ArtifactEntry>? ArtifactEntries { get; set; } = [];
}
