namespace OpenArchival.Core.Remove;

public class CategoryFieldOption
{
    public required int CategoryId { get; set; }
    public required int FieldNumber { get; set; }
    public required string Value { get; set; } 
    public required string Name { get; set; }
    public string? Description { get; set; }
}
