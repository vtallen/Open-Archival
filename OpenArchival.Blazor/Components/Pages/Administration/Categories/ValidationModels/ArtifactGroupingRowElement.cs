namespace OpenArchival.Blazor;

public class ArtifactGroupingRowElement
{
    public required int Id { get; set; }

    public required string ArtifactGroupingIdentifier { get; set; }

    public required string CategoryName { get; set; }

    public required string Title { get; set; }

    public bool IsPublicallyVisible { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ArtifactGroupingRowElement;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}