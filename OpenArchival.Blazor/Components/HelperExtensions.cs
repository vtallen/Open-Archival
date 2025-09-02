using Microsoft.AspNetCore.Components;

namespace OpenArchival.Blazor;

public static class HelperExtensions
{
    public static void ReloadPage(this NavigationManager manager)
    {
        manager.NavigateTo(manager.Uri, true);
    }
}
