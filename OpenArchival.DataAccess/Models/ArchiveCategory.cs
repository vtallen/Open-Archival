using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OpenArchival.DataAccess;

public class ArchiveCategory
{
    [Key]
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique =true)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string FieldSeparator { get; set; } = "-";

    public List<string> FieldNames { get; set; } = [];

    public List<string> FieldDescriptions { get; set; } = [];
}
