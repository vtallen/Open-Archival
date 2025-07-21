using OpenArchival.Database;
using OpenArchival.Database.Category;
using System.ComponentModel.DataAnnotations;

public class CategoryFieldValidationModel
{
    [Required(ErrorMessage = "A field name must be provided.")]
    public string FieldName { get; set; } = "";

    public string Description { get; set; } = "";
}