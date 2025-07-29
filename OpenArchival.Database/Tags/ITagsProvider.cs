using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface ITagsProvider
{
    public Task<bool> AddTag(string tag);

    public Task<bool> RemoveTag(string tag);

    public Task<bool> ContainsTag(string tag);

    public Task<IEnumerable<string>> TopTags(int resultCount);

    public Task<IEnumerable<string>> SearchTags(string query);
}
