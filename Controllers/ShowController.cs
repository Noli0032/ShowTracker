using Microsoft.AspNetCore.Mvc;
using MyProject.Services;

namespace MyProject.Controllers;

public class ShowController : Controller
{
    private readonly ITvMazeService _tvMazeService;
    public ShowController(ITvMazeService tvMazeService)
    {
        _tvMazeService = tvMazeService;
    }

    public IActionResult Index()
    {
        return View();
    }
}