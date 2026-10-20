using Microsoft.Extensions.Caching.Memory;
using ShowTracker.Models;
using ShowTracker.ViewModels;

namespace ShowTracker.Services;

public class HomeService : IHomeService
{
    private const string HomePageCacheKey = "home_page";
    private const string LastIndexPageCacheKey = "last_index_page";
    private const int RowSize = 20;
    // The first pages hold the oldest shows, which have the most complete weight and rating data
    private const int SampledPages = 6;
    // New shows sit on the last pages of the TvMaze show index
    private const int NewestPages = 3;
    // The index has around 380 pages. The last page is found by walking up from this page
    private const int LastPageSearchStart = 340;
    private const int LastPageSearchStep = 10;

    private static readonly SemaphoreSlim BuildLock = new(1, 1);

    private readonly ITvMazeService _tvMazeService;
    private readonly IMemoryCache _memoryCache;

    public HomeService(ITvMazeService tvMazeService, IMemoryCache memoryCache)
    {
        _tvMazeService = tvMazeService;
        _memoryCache = memoryCache;
    }

    public async Task<HomePageViewModel> GetHomePageAsync()
    {
        if (_memoryCache.TryGetValue(HomePageCacheKey, out HomePageViewModel? cached))
        {
            return cached!;
        }

        // Only one request builds the rows, others wait and then read the cache
        await BuildLock.WaitAsync();
        try
        {
            if (_memoryCache.TryGetValue(HomePageCacheKey, out cached))
            {
                return cached!;
            }

            var homePage = await BuildHomePageAsync();

            // Do not keep an empty result for long, so a failed API call is retried soon
            var hasShows = homePage.MostPopular.Length > 0 || homePage.NewAndRunning.Length > 0;
            _memoryCache.Set(HomePageCacheKey, homePage, hasShows ? TimeSpan.FromHours(24) : TimeSpan.FromMinutes(1));

            return homePage;
        }
        finally
        {
            BuildLock.Release();
        }
    }

    private async Task<HomePageViewModel> BuildHomePageAsync()
    {
        var sampledShows = new List<TvShow>();
        for (var page = 0; page < SampledPages; page++)
        {
            var shows = await _tvMazeService.GetTvShowsByPageAsync(page);
            if (shows.Length == 0) break;
            sampledShows.AddRange(shows);
        }

        var newestShows = new List<TvShow>();
        var lastPage = await FindLastPageAsync();
        for (var page = Math.Max(SampledPages, lastPage - NewestPages + 1); page <= lastPage; page++)
        {
            newestShows.AddRange(await _tvMazeService.GetTvShowsByPageAsync(page));
        }

        var sampledWithImage = sampledShows.Where(HasImage).ToList();

        return new HomePageViewModel
        {
            MostPopular = sampledWithImage
                .OrderByDescending(show => show.Weight ?? 0)
                .ThenByDescending(show => show.Rating?.Average ?? 0)
                .Take(RowSize)
                .ToArray(),
            HighestRated = sampledWithImage
                .Where(show => show.Rating?.Average != null)
                .OrderByDescending(show => show.Rating!.Average)
                .Take(RowSize)
                .ToArray(),
            // New shows often have no rating yet, so only an image is required
            NewAndRunning = newestShows
                .Where(HasImage)
                .Where(show => show.Status == "Running")
                .OrderByDescending(show => show.Premiered)
                .Take(RowSize)
                .ToArray()
        };
    }

    private async Task<int> FindLastPageAsync()
    {
        if (!_memoryCache.TryGetValue(LastIndexPageCacheKey, out int lastKnownPage))
        {
            lastKnownPage = LastPageSearchStart;
        }

        // Walk upwards until a page is empty, then narrow down the exact last page
        var low = lastKnownPage;
        var high = low + LastPageSearchStep;
        while (await PageHasShowsAsync(high))
        {
            low = high;
            high += LastPageSearchStep;
        }

        while (high - low > 1)
        {
            var middle = (low + high) / 2;
            if (await PageHasShowsAsync(middle)) low = middle;
            else high = middle;
        }

        _memoryCache.Set(LastIndexPageCacheKey, low, TimeSpan.FromDays(30));
        return low;
    }

    private async Task<bool> PageHasShowsAsync(int page)
    {
        var shows = await _tvMazeService.GetTvShowsByPageAsync(page);
        return shows.Length > 0;
    }

    private static bool HasImage(TvShow show) => show.Image?.Medium != null;
}
