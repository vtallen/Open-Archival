using Microsoft.AspNetCore.Identity;

namespace OpenArchival.DataAccess;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string PermissionLevel { get; set; } = "";
}
