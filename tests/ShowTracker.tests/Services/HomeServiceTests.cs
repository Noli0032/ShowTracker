using Microsoft.Extensions.Caching.Memory;
using Moq;
using ShowTracker.Models;
using ShowTracker.Services;

namespace ShowTracker.tests;

[TestClass]
public sealed class HomeServiceTests
{
    private static HomeService CreateService(Func<int, TvShow[]> pageProvider, out Mock<ITvMazeService> tvMazeMock)
    {
        tvMazeMock = new Mock<ITvMazeService>();
        tvMazeMock
            .Setup(service => service.GetTvShowsByPageAsync(It.IsAny<int>()))
            .ReturnsAsync((int page) => pageProvider(page));

        return new HomeService(tvMazeMock.Object, new MemoryCache(new MemoryCacheOptions()));
    }

    // Page 0 holds the sampled shows, pages 340 to lastPage form the end of the index with the new shows on the last page
    private static Func<int, TvShow[]> CreateIndex(TvShow[] sampledShows, TvShow[] newestShows, int lastPage = 342)
    {
        return page =>
        {
            if (page == 0) return sampledShows;
            if (page == lastPage) return newestShows;
            if (page >= 340 && page < lastPage) return [CreateShow(9000 + page, "Filler", hasImage: false)];
            return [];
        };
    }

    private static TvShow CreateShow(
        int id,
        string name,
        int? weight = null,
        float? rating = null,
        string status = "Ended",
        DateOnly? premiered = null,
        bool hasImage = true)
    {
        return new TvShow
        {
            Id = id,
            Name = name,
            Language = "English",
            Status = status,
            Weight = weight,
            Premiered = premiered,
            Rating = rating == null ? null : new TvShowRating { Average = rating },
            Image = hasImage ? new TvShowImage { Medium = $"https://static.tvmaze.com/{id}.jpg" } : null
        };
    }

    [TestMethod]
    public async Task GetHomePageAsync_MostPopular_IsSortedByWeightDescending()
    {
        // Arrange
        var sampled = new[]
        {
            CreateShow(1, "Low", weight: 50),
            CreateShow(2, "High", weight: 100),
            CreateShow(3, "Medium", weight: 80)
        };
        var service = CreateService(CreateIndex(sampled, []), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        CollectionAssert.AreEqual(new[] { "High", "Medium", "Low" }, result.MostPopular.Select(show => show.Name).ToArray());
    }

    [TestMethod]
    public async Task GetHomePageAsync_HighestRated_IsSortedByRatingAndSkipsUnratedShows()
    {
        // Arrange
        var sampled = new[]
        {
            CreateShow(1, "Good", rating: 7.5f),
            CreateShow(2, "Unrated"),
            CreateShow(3, "Best", rating: 9.1f)
        };
        var service = CreateService(CreateIndex(sampled, []), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        CollectionAssert.AreEqual(new[] { "Best", "Good" }, result.HighestRated.Select(show => show.Name).ToArray());
    }

    [TestMethod]
    public async Task GetHomePageAsync_ShowsWithoutImage_AreExcludedFromAllRows()
    {
        // Arrange
        var sampled = new[]
        {
            CreateShow(1, "No image", weight: 100, rating: 9.5f, hasImage: false),
            CreateShow(2, "With image", weight: 10, rating: 5.0f)
        };
        var newest = new[]
        {
            CreateShow(3, "New no image", status: "Running", hasImage: false)
        };
        var service = CreateService(CreateIndex(sampled, newest), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        Assert.HasCount(1, result.MostPopular);
        Assert.HasCount(1, result.HighestRated);
        Assert.IsEmpty(result.NewAndRunning);
    }

    [TestMethod]
    public async Task GetHomePageAsync_NewAndRunning_OnlyContainsRunningShowsNewestFirst()
    {
        // Arrange
        var newest = new[]
        {
            CreateShow(1, "Older", status: "Running", premiered: new DateOnly(2025, 1, 1)),
            CreateShow(2, "Ended", status: "Ended", premiered: new DateOnly(2025, 6, 1)),
            CreateShow(3, "Newer", status: "Running", premiered: new DateOnly(2025, 9, 1))
        };
        var service = CreateService(CreateIndex([], newest), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        CollectionAssert.AreEqual(new[] { "Newer", "Older" }, result.NewAndRunning.Select(show => show.Name).ToArray());
    }

    [TestMethod]
    public async Task GetHomePageAsync_WhenIndexHasGrown_FindsNewestShowsOnTheLastPage()
    {
        // Arrange
        var newest = new[]
        {
            CreateShow(1, "Brand new", status: "Running", premiered: new DateOnly(2026, 1, 1))
        };
        var service = CreateService(CreateIndex([], newest, lastPage: 357), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        Assert.HasCount(1, result.NewAndRunning);
        Assert.AreEqual("Brand new", result.NewAndRunning[0].Name);
    }

    [TestMethod]
    public async Task GetHomePageAsync_WithManyShows_LimitsEachRowToTwentyShows()
    {
        // Arrange
        var sampled = Enumerable.Range(1, 30)
            .Select(id => CreateShow(id, $"Show {id}", weight: id, rating: 5.0f))
            .ToArray();
        var service = CreateService(CreateIndex(sampled, []), out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        Assert.HasCount(20, result.MostPopular);
        Assert.HasCount(20, result.HighestRated);
    }

    [TestMethod]
    public async Task GetHomePageAsync_WhenApiReturnsNoShows_ReturnsEmptyRows()
    {
        // Arrange
        var service = CreateService(_ => [], out _);

        // Act
        var result = await service.GetHomePageAsync();

        // Assert
        Assert.IsEmpty(result.MostPopular);
        Assert.IsEmpty(result.HighestRated);
        Assert.IsEmpty(result.NewAndRunning);
    }

    [TestMethod]
    public async Task GetHomePageAsync_WhenCalledTwice_FetchesFromTheApiOnlyOnce()
    {
        // Arrange
        var sampled = new[] { CreateShow(1, "Show", weight: 90, rating: 8.0f) };
        var service = CreateService(CreateIndex(sampled, []), out var tvMazeMock);

        // Act
        await service.GetHomePageAsync();
        await service.GetHomePageAsync();

        // Assert
        tvMazeMock.Verify(tvMaze => tvMaze.GetTvShowsByPageAsync(0), Times.Once);
    }
}
