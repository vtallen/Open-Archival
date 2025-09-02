using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenArchival.DataAccess;

public class ListedName
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {  get; set; }
    
    public required ArtifactEntry ParentArtifactEntry { get; set; }
    
    public required string Value { get; set; }
}
