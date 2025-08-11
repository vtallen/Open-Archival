namespace OpenArchival.Blazor;

using Microsoft.IdentityModel.Tokens;
using OpenArchival.DataAccess;
using System.ComponentModel.DataAnnotations;

public class ArtifactEntryValidationModel : IValidatableObject
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "An artifact numbering must be supplied")]
    public string? ArtifactNumber { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A title must be provided")]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public required string? StorageLocation { get; set; }

    public List<string>? Tags { get; set; } = [];

    public List<string>? ListedNames { get; set; } = [];

    public List<DateTime>? AssociatedDates { get; set; } = [];

    public List<string>? Defects { get; set; } = [];

    public List<string>? Links { get; set; } = [];

    public string? ArtifactType { get; set; }   

    public List<FilePathListing>? Files { get; set; } = [];

    public Dictionary<string, string>? FileTextContent { get; set; } = [];
    
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Links.IsNullOrEmpty() && Files.IsNullOrEmpty())
        {
            yield return new ValidationResult(
            "Either uploaded files or add content links",
            new[] {nameof(Links), nameof(Files)} 
            ); 
        }
    }

    public bool IsPublicallyVisible { get; set; }
}
