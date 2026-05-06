namespace MyProject.Areas.Identity.Data;

using Microsoft.AspNetCore.Identity;
using MyProject.Models.UserShow;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserShowEntry> UserShowEntries {get;} = [];
}