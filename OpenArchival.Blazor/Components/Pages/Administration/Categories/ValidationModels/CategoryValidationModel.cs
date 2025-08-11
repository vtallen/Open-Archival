namespace OpenArchival.Blazor;

using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using OpenArchival.DataAccess;

using System.ComponentModel.DataAnnotations;

public class CategoryValidationModel
{
    public int? DatabaseId { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Field separator is required.")]
    [StringLength(1, ErrorMessage = "Separator must be a single character.")]
    public string FieldSeparator { get; set; } = "-";

    [Required(ErrorMessage = "At least one field is needed")]
    [Range(1, int.MaxValue, ErrorMessage = "At least one field must be created.")]
    public int NumFields { get; set; } = 1;

    public List<string> FieldNames { get; set; } = [""];

    public List<string> FieldDescriptions { get; set; } = [""];

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (FieldNames.IsNullOrEmpty() || FieldDescriptions.IsNullOrEmpty())
        {
            yield return new ValidationResult(
            "Either the FieldNames or FieldDescriptions were null or empty. At least one is required",
            new[] { nameof(FieldNames), nameof(FieldDescriptions) }
            );
        }
    }

    public static CategoryValidationModel FromArchiveCategory(ArchiveCategory category)
    {
        return new CategoryValidationModel()
        {
            Name = category.Name,
            Description = category.Description,
            DatabaseId = category.Id,
            FieldSeparator = category.FieldSeparator,
            FieldNames = category.FieldNames,
            FieldDescriptions = category.FieldDescriptions,
        };
    }

    public static ArchiveCategory ToArchiveCategory(CategoryValidationModel model)
    {
        return new ArchiveCategory()
        {
            Name = model.Name,
            FieldSeparator = model.FieldSeparator,
            Description = model.Description,
            FieldNames = model.FieldNames,
            FieldDescriptions = model.FieldDescriptions
        };
    }

    public static void UpdateArchiveValidationModel(CategoryValidationModel model, ArchiveCategory category)
    {
        category.Name = model.Name ?? throw new ArgumentNullException(nameof(model.Name), "The model name was null.");
        category.Description = model.Description; 
        category.FieldSeparator = model.FieldSeparator;
        category.FieldNames = model.FieldNames;
        category.FieldDescriptions = model.FieldDescriptions;
    }
}