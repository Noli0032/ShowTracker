namespace ShowTracker.Areas.Identity.Data;

using Microsoft.AspNetCore.Identity;
using ShowTracker.Models.UserShow;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserShowEntry> UserShowEntries {get;} = [];
}