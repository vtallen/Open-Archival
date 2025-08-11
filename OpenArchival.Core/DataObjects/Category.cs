using System.Runtime.CompilerServices;

namespace OpenArchival.Core.Remove;

public class Category
{
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string FieldSeparator { get; set; }
    public required string[] FieldNames { get; set; }
    public required string[] FieldDescriptions { get; set; }

    public IEnumerable<KeyValuePair<string, string>> FieldsIterator
    {
        get
        {
            for (int index = 0; index < FieldNames.Length; ++index)
            {
                yield return new KeyValuePair<string, string>(FieldNames[index], FieldDescriptions[index]);
            }
        }
    }
}
