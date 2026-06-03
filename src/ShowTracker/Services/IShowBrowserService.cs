using ShowTracker.Models;

namespace ShowTracker.Services;

public interface IShowBrowserService
{
    Task<TvShow[]> GetShowsForPageAsync(int page);
}