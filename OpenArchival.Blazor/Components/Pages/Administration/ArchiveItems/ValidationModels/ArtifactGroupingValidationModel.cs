using Microsoft.IdentityModel.Tokens;
using OpenArchival.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace OpenArchival.Blazor;

public class ArtifactGroupingValidationModel : IValidatableObject
{
    [Required(ErrorMessage = "A grouping title is required.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "A grouping description is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "A type is required.")]
    public string? Type { get; set; }

    public ArchiveCategory? Category { get; set; }

    public List<IdentifierFieldValidationModel> IdentifierFieldValues { get; set; } = new();

    public List<ArtifactEntryValidationModel> ArtifactEntries { get; set; } = new();

    public bool IsPublicallyVisible { get; set; }
    
    public ArtifactGrouping ToArtifactGrouping()
    {
        IdentifierFields identifierFields = new();
        identifierFields.Values = IdentifierFieldValues.Select(p => p.Value).ToList();

        List<ArtifactEntry> entries = [];
        foreach (var entry in ArtifactEntries)
        {
            entries.Add(entry.ToArtifactEntry());
        }

        var grouping = new ArtifactGrouping()
        {
            Title = Title,
            Description = Description,
            Category = Category,
            IdentifierFields = identifierFields,
            IsPublicallyVisible = true,
            ChildArtifactEntries = entries,
            Type = Type
        };

        // Create the parent link
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            entry.ArtifactGrouping = grouping;
        }

        return grouping;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var entry in ArtifactEntries)
        {
            var context = new ValidationContext(entry);
            var validationResult = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(entry, context, validationResult);
            foreach (var result in validationResult)
            {
                yield return result;
            }
        }

        if (ArtifactEntries.IsNullOrEmpty())
        {
            yield return new ValidationResult("Must upload one or more files");
        }
    }
}
