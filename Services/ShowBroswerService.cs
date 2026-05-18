using Microsoft.Extensions.Caching.Memory;
using MyProject.Models;

namespace MyProject.Services;

public class ShowBrowserService : IShowBrowserService
{
    private readonly ITvMazeService _tvMazeService;
    private readonly IMemoryCache _memoryCache;
    private const string ShowsCacheKey = "accumulated_shows";
    private const string LastPageCacheKey = "last_tvmaze_page";

    public ShowBrowserService (ITvMazeService tvMazeService, IMemoryCache memoryCache)
    {
        _tvMazeService = tvMazeService;
        _memoryCache = memoryCache;
    }

    public async Task<TvShow[]> GetShowsForPageAsync(int page)
    {
        // Calculate total amount of shows needed to fetch
        var showAmount = page * 72;
        if(_memoryCache.TryGetValue(ShowsCacheKey, out List<TvShow>? cachedShows))
        {
            // Fetch shows until we have enough to show
            while (cachedShows!.Count < showAmount)
            {
                _memoryCache.TryGetValue(LastPageCacheKey, out int lastPage);
                TvShow[] nextPageShows = await _tvMazeService.GetTvShowsByPageAsync(lastPage + 1);

                // Reached the end of TvMaze shows
                if(nextPageShows.Length == 0) break;

                cachedShows!.AddRange(nextPageShows.Where(show => show.Image?.Medium != null));
                _memoryCache.Set(LastPageCacheKey, lastPage + 1, TimeSpan.FromHours(24));
                _memoryCache.Set(ShowsCacheKey, cachedShows, TimeSpan.FromHours(24));
            }

            return cachedShows
                .Skip((page - 1) * 72)
                .Take(72)
                .ToArray();
        }
        else
        {
            cachedShows = new List<TvShow>();
            TvShow[] firstPage = await _tvMazeService.GetTvShowsByPageAsync(0);
            cachedShows.AddRange(firstPage.Where(show => show.Image?.Medium != null));
            _memoryCache.Set(LastPageCacheKey, 0, TimeSpan.FromHours(24));
            _memoryCache.Set(ShowsCacheKey, cachedShows, TimeSpan.FromHours(24));

            return cachedShows
                .Take(72)
                .ToArray();
        }
    }
}