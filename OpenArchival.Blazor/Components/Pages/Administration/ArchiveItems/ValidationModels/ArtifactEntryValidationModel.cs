namespace OpenArchival.Blazor;

using Microsoft.IdentityModel.Tokens;
using OpenArchival.DataAccess;
using System.ComponentModel.DataAnnotations;

public class ArtifactEntryValidationModel 
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "An artifact numbering must be supplied")]
    public string? ArtifactNumber { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A title must be provided")]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? StorageLocation { get; set; }

    public List<string>? Tags { get; set; } = [];

    public List<string>? ListedNames { get; set; } = [];

    public List<DateTime>? AssociatedDates { get; set; } = [];

    public List<string>? Defects { get; set; } = [];

    public List<string>? Links { get; set; } = [];

    public List<FilePathListing>? Files { get; set; } = [];

    public string? FileTextContent { get; set; } 

    public List<ArtifactEntry> RelatedArtifacts { get; set; } = [];
   
    /*
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        
    }
    */
    public bool IsPublicallyVisible { get; set; }

    public ArtifactEntry ToArtifactEntry(ArtifactGrouping? parent = null)
    {
        List<ArtifactEntryTag> tags = new();
        if (Tags is not null)
        {
            foreach (var tag in Tags)
            {
                tags.Add(new ArtifactEntryTag() { Name = tag });
            }
        }

        List<ArtifactDefect> defects = new();
        foreach (var defect in Defects)
        {
            defects.Add(new ArtifactDefect() { Description=defect});
        }

        

        var entry = new ArtifactEntry()
        {
            Files = Files,
            Type = new DataAccess.ArtifactType() { Name = Type },
            ArtifactNumber = ArtifactNumber,
            AssociatedDates = AssociatedDates,
            Defects = defects, 
            Links = Links,
            StorageLocation = null,
            Description = Description,
            FileTextContent = FileTextContent,
            IsPubliclyVisible = IsPublicallyVisible,
            Tags = tags,
            Title = Title,
            ArtifactGrouping = parent,
            RelatedTo = RelatedArtifacts,
        };

        List<ListedName> listedNames = new();
        foreach (var name in ListedNames)
        {
            listedNames.Add(new ListedName() { Value=name });
        }

        entry.ListedNames = listedNames;

        if (!string.IsNullOrEmpty(StorageLocation))
        {
            entry.StorageLocation = new ArtifactStorageLocation() { Location = StorageLocation };
        }

        return entry;
    }
}
