using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyProject.Areas.Identity.Data;
using MyProject.Models.UserShow;
using MyProject.Services;

namespace MyProject.Controllers;

public class UserController : Controller
{
    private readonly IShowEntryService _showEntryService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserController(IShowEntryService showEntryService, UserManager<ApplicationUser> userManager)
    {
        _showEntryService = showEntryService;
        _userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> Watchlist()
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        string userId = _userManager.GetUserId(User)!;
        List<UserShowEntry> watchlist = await _showEntryService.GetWatchListAsync(userId);
        return View(watchlist);
    }
}