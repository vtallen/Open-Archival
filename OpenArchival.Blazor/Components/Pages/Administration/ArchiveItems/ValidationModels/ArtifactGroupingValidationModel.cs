using Microsoft.IdentityModel.Tokens;
using OpenArchival.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace OpenArchival.Blazor;

public class ArtifactGroupingValidationModel : IValidatableObject
{
    /// <summary>
    /// Used by update code to track the database record that corresponds to the data within this DTO
    /// </summary>
    public int? Id { get; set; }  

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
            Id = Id ?? default,
            Title = Title,
            Description = Description,
            Category = Category,
            IdentifierFields = identifierFields,
            IsPublicallyVisible = true,
            ChildArtifactEntries = entries,
            Type = new ArtifactType() { Name = Type }
        };

        // Create the parent link
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            entry.ArtifactGrouping = grouping;
        }

        return grouping;
    }

    public static ArtifactGroupingValidationModel ToValidationModel(ArtifactGrouping grouping)
    {
        var entries = new List<ArtifactEntryValidationModel>();

        foreach (var entry in grouping.ChildArtifactEntries)
        {
            var defects = new List<string>();
            
            if (entry.Defects is not null)
            {
                defects.AddRange(entry.Defects.Select(defect => defect.Description));
            }

            var validationModel = new ArtifactEntryValidationModel() 
            { 
                Title = entry.Title,
                StorageLocation = entry.StorageLocation.Location,
                ArtifactNumber = entry.ArtifactNumber,
                AssociatedDates = entry.AssociatedDates,
                Defects = entry?.Defects?.Select(defect => defect.Description).ToList(),
                Description = entry?.Description,
                Files = entry?.Files,
                FileTextContent = entry?.FileTextContent,
                IsPublicallyVisible = entry.IsPubliclyVisible,
                Links = entry.Links,
                ListedNames = entry?.ListedNames?.Select(name => name.Value).ToList(),
                RelatedArtifacts = entry.RelatedTo,
                Tags = entry?.Tags?.Select(tag => tag.Name).ToList(),
                Type = entry?.Type.Name,
            };

            entries.Add(validationModel);
        }

        var identifierFieldsStrings = grouping.IdentifierFields.Values;
        List<IdentifierFieldValidationModel> identifierFields = new();
        for (int index = 0; index < identifierFieldsStrings.Count; ++index) 
        {
            identifierFields.Add(new IdentifierFieldValidationModel() 
            { 
                Value = identifierFieldsStrings[index], 
                Name = grouping.Category.FieldNames[index] 
            });
        }
        return new ArtifactGroupingValidationModel()
        {
            Id = grouping.Id,
            Title = grouping.Title,
            ArtifactEntries = entries,
            Category = grouping.Category,
            Description = grouping.Description,
            IdentifierFieldValues = identifierFields, 
            IsPublicallyVisible = grouping.IsPublicallyVisible,
            Type = grouping.Type.Name 
        };
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
