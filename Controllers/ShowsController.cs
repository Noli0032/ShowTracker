using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MyProject.Areas.Identity.Data;
using MyProject.Models;
using MyProject.Services;
using MyProject.ViewModels;

namespace MyProject.Controllers;

public class ShowsController : Controller
{
    private readonly ITvMazeService _tvMazeService;
    private readonly IShowEntryService _showEntryService;
    private readonly UserManager<ApplicationUser> _userManager;
    public ShowsController(ITvMazeService tvMazeService, IShowEntryService showEntryService, UserManager<ApplicationUser> userManager)
    {
        _tvMazeService = tvMazeService;
        _showEntryService = showEntryService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        TvShow[] tvShows = await _tvMazeService.GetTvShowsByPageAsync(0);
        return View(tvShows);
    }

    public async Task<IActionResult> Details(int id)
    {
        TvShow? tvShow = await _tvMazeService.GetTvShowDetails(id);
        if (tvShow == null)
        {
            return NotFound();
        }
        var userId = _userManager.GetUserId(User);

        // A user which is not logged is still allowed to view show details, therefore this will return false in that case
        bool isInWatchList = userId != null && await _showEntryService.IsInWatchlist(userId, id);

        ShowDetailsViewModel detailsViewModel = new ShowDetailsViewModel{
            Show = tvShow,
            IsInWatchList = isInWatchList
        };

        return View(detailsViewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> WatchlistAdd(int tvMazeShowId)
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        var userId = _userManager.GetUserId(User)!;
        await _showEntryService.AddToWatchlist(userId, tvMazeShowId);
        return RedirectToAction("Details", new {id = tvMazeShowId});
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> WatchlistRemove(int tvMazeShowId)
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        var userId = _userManager.GetUserId(User)!;
        await _showEntryService.RemoveFromWatchlist(userId, tvMazeShowId);
        return RedirectToAction("Details", new {id = tvMazeShowId});
    }
}