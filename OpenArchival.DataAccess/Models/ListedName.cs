using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class ListedName
{
    [Key]
    public required int Id {  get; set; }
    
    public required ArtifactEntry ParentArtifactEntry { get; set; }
    
    public string? Title { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
