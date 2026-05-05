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
        TvShow[] tvShows = await _tvMazeService.GetWrapperTvShowsByPageAsync(0);
        return View(tvShows);
    }

    public IActionResult Details(int id)
    {
        return View();
    }
}