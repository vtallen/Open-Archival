using System.ComponentModel.DataAnnotations;
using OpenArchival.DataAccess;

namespace OpenArchival.Blazor;

public class ArchiveItemValidationModel
{
    [Required(ErrorMessage = "A category is required", AllowEmptyStrings = false)]
    public string Category { get; set; } = "";

    [Required(ErrorMessage = "An item identifier is required", AllowEmptyStrings = false)]
    public List<IdentifierFieldValidationModel> IdentifierFields { get; set; } = new();

    public string Identifier { get; set; } = "";

    [Required(ErrorMessage = "An item title is required", AllowEmptyStrings = false)]
    public string Title { get; set; } = "";

    public string? Description { get; set; } 

    public string? StorageLocation { get; set; }
    
    public string? ArtifactType { get; set; }

    public List<string> Tags { get; set; } = new();

    public List<string> AssociatedNames { get; set; } = new();

    public List<DateTime> AssociatedDates { get; set; } = new();
    
    public List<string> Defects { get; set; } = new();

    public List<string> RelatedArtifacts { get; set; } = new();

    public bool IsPublic { get; set; } = true;

}
