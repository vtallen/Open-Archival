using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class ArtifactEntry
{
    [Key]
    public required int Id { get; set; }
    
    /// <summary>
    /// This value gets appended on the end of the contianing ArtifactGrouping's 
    /// Category value
    /// </summary>
    public string? ArtifactIdentifier 
    {
        get
        {
            return (ParentArtifactGrouping is not null) 
                ? ModelHelpers.MakeIdentifier(ParentArtifactGrouping.IdentifierFields.Values, ParentArtifactGrouping.Category.FieldSeparator, ArtifactNumber) 
                : null;
        }
    }

    public string? ArtifactNumber { get; set; } 

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required ArtifactStorageLocation StorageLocation { get; set; }

    public List<ArtifactEntryTag>? Tags { get; set; } 

    public List<string>? ListedNames { get; set; } 

    public List<DateTime>? AssociatedDates { get; set; }
    
    public List<string>? Defects { get; set; }

    public List<string>? Links { get; set; }
    
    public ArtifactGrouping? ParentArtifactGrouping { get; set; }

    public required List<FilePathListing> Files { get; set; }
    
    /// <summary>
    /// Maps the file name to the textual contents of the file 
    /// </summary>
    public Dictionary<string, string>? FileTextContent { get; set; } = null; 

    public bool IsPubliclyVisible { get; set; }
}

