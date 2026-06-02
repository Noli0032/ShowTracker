using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowTracker.Areas.Identity.Data;
using ShowTracker.Models.UserShow;
using ShowTracker.Services;

namespace ShowTracker.Controllers;

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
        List<UserShowEntry> watchlist = await _showEntryService.GetWatchlistAsync(userId);
        return View(watchlist);
    }

    [Authorize]
    public async Task<IActionResult> Watched()
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        string userId = _userManager.GetUserId(User)!;
        List<UserShowEntry> watched = await _showEntryService.GetWatchedAsync(userId);
        return View(watched);
    }
}