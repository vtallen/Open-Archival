using OpenArchival.Database.Category;
using System.ComponentModel.DataAnnotations;

public class CategoryModel
{
    [Required(ErrorMessage = "Category name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Field separator is required.")]
    [StringLength(1, ErrorMessage = "Separator must be a single character.")]
    public string FieldSeparator { get; set; } = "-";

    [Required(ErrorMessage = "At least one field is needed")]
    [Range(1, int.MaxValue, ErrorMessage = "At least one field must be created.")]
    public int NumFields { get; set; } = 1;

    public List<string> FieldNames { get; set; } = [""];

    public List<string> FieldDescriptions { get; set; } = [""]; 

    public Category ToCategory()
    {
        return new Category() { CategoryName = Name, FieldSeparator = FieldSeparator, FieldNames = FieldNames.ToArray(), FieldDescriptions = FieldDescriptions.ToArray() };
    }
    public static CategoryModel FromCategory(Category category)
    {
        return new CategoryModel() { Name = category.CategoryName, FieldSeparator=category.FieldSeparator, NumFields=category.FieldNames.Length, FieldNames = new(category.FieldNames), FieldDescriptions = new(category.FieldDescriptions)};
    }
}