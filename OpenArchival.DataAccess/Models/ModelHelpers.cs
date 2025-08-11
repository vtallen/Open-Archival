namespace OpenArchival.DataAccess;

public class ModelHelpers
{
    public static string? MakeIdentifier(List<string>? values, string fieldSeperator, string? archiveEntryNumber)
    {
        if (values is null || values.Count == 0) return null;

        return (archiveEntryNumber is not null) ? $"{string.Join(fieldSeperator, values)}{archiveEntryNumber}" : string.Join(fieldSeperator, values);
    }
}
