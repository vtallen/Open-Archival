using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IArtifactTypesProvider
{
    public Task<bool> ContainsType(string type);

    public Task<bool> AddType(string type);

    public Task<bool> RemoveType(string type);

    public Task<IEnumerable<string>> SearchTypes(string query);

    public Task<IEnumerable<string>> TopTypes(int resultCount);
}
