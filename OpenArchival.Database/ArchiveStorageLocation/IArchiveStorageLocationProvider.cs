using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IArchiveStorageLocationProvider
{
    public Task<bool> ContainsLocation(string location);

    public Task<bool> AddLocation(string location);

    public Task<bool> RemoveLocation(string location);

    public Task<IEnumerable<string>> SearchLocations(string query);

    public Task<IEnumerable<string>> TopLocations(int resultCount);
}
