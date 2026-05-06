using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MyProject.Areas.Identity.Data;
using MyProject.Models;
using MyProject.Services;

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
        return View(tvShow);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> WatchList(int tvMazeShowId)
    {
        var userId = _userManager.GetUserId(User)!;
        await _showEntryService.AddToWatchList(userId, tvMazeShowId);
        return RedirectToAction("Details", new {id = tvMazeShowId});
    }
}