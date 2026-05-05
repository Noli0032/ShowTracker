using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services;

namespace MyProject.Controllers;

public class ShowController : Controller
{
    private readonly ITvMazeService _tvMazeService;
    public ShowController(ITvMazeService tvMazeService)
    {
        _tvMazeService = tvMazeService;
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
}