using System.ComponentModel.DataAnnotations;

namespace OpenArchival.DataAccess;

public class ArchiveCategory
{
    [Key]
    public int? Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string FieldSeparator { get; set; } = "-";

    public List<string> FieldNames { get; set; } = [];

    public List<string> FieldDescriptions { get; set; } = [];
}
