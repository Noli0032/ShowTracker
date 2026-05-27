using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Identity.Data;
using MyProject.Models;
using MyProject.Models.UserShow;
using MyProject.Services;
using MyProject.ViewModels;

namespace MyProject.Controllers;

public class ShowsController : Controller
{
    private readonly ITvMazeService _tvMazeService;
    private readonly IShowEntryService _showEntryService;
    private readonly IShowBrowserService _showBrowserService;
    private readonly UserManager<ApplicationUser> _userManager;
    public ShowsController(ITvMazeService tvMazeService, IShowEntryService showEntryService, IShowBrowserService showBrowserService, UserManager<ApplicationUser> userManager)
    {
        _tvMazeService = tvMazeService;
        _showEntryService = showEntryService;
        _showBrowserService = showBrowserService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        TvShow[] tvShows = await _showBrowserService.GetShowsForPageAsync(page);

        ShowPageViewModel pageViewModel = new ShowPageViewModel
        {
            TvShows = tvShows,
            Page = page
        };
        return View(pageViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        TvShow? tvShow = await _tvMazeService.GetTvShowDetailsAsync(id);
        if (tvShow == null)
        {
            return NotFound();
        }
        var userId = _userManager.GetUserId(User);

        ShowStatus? showStatus = userId != null ? await _showEntryService.GetShowStatusAsync(userId, id) : null;

        ShowDetailsViewModel detailsViewModel = new ShowDetailsViewModel{
            Show = tvShow,
            ShowStatus = showStatus 
        };

        return View(detailsViewModel);
    }

    public async Task<IActionResult> Episodes(int id)
    {
        TvShowEpisode[] tvShowEpisodes = await _tvMazeService.GetTvShowEpisodesAsync(id);
        var episodesBySeason = tvShowEpisodes
            .GroupBy(e => e.Season)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());

        return View(episodesBySeason);
    }

    public async Task<IActionResult> Search(string query)
    {
        TvShow[] tvShows = await _tvMazeService.SearchTvShowsAsync(query);
        return View(tvShows);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(ShowStatus showStatus, int tvMazeShowId)
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        var userId = _userManager.GetUserId(User)!;
        await _showEntryService.UpsertShowEntryAsync(userId, tvMazeShowId, showStatus);
        return RedirectToAction("Details", new {id = tvMazeShowId});
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ShowEntryDelete(int tvMazeShowId)
    {
        // Since we have the authorize attribute, we should be certain that this is not null
        var userId = _userManager.GetUserId(User)!;
        await _showEntryService.RemoveUserEntryAsync(userId, tvMazeShowId);
        return RedirectToAction("Details", new {id = tvMazeShowId});
    }
}