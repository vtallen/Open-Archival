using System.Linq;
using Microsoft.Extensions.Logging;
using OpenArchival.DataAccess;

namespace OpenArchival.Controller;

public class AddArchiveGroupingController
{
    private ILogger<AddArchiveGroupingController> _logger;

    private IArchiveCategoryProvider  _categoryProvider; 
    
    public AddArchiveGroupingController(ILogger<AddArchiveGroupingController> logger, IArchiveCategoryProvider archiveCategoryProvider) 
    { 
        _logger = logger;
        _categoryProvider = archiveCategoryProvider;
    }
 }

