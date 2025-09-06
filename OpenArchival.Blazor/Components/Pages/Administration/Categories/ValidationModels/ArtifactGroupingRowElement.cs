namespace OpenArchival.Blazor;

public class ArtifactGroupingRowElement
{
    public required int Id { get; set; }

    public required string ArtifactGroupingIdentifier { get; set; }

    public required string CategoryName { get; set; }

    public required string Title { get; set; }

    public bool IsPublicallyVisible { get; set; }

    public bool Equals(ArtifactGroupingRowElement? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id; // Compare based on the unique Id
    }

    public override bool Equals(object? obj) => Equals(obj as ArtifactGroupingRowElement);

    public override int GetHashCode() => Id.GetHashCode();
}