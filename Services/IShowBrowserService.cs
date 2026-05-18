using MyProject.Models;

namespace MyProject.Services;

public interface IShowBrowserService
{
    Task<TvShow[]> GetShowsForPageAsync(int page);
}