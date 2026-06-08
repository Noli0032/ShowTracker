using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ShowTracker.Models;
using ShowTracker.Services;
using ShowTracker.Tests.Helpers;

namespace ShowTracker.tests;

[TestClass]
public sealed class TvMazeServiceTests
{
    private static TvMazeService CreateService(string json)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://api.tvmaze.com")
        };

        var logger = NullLogger<TvMazeService>.Instance;
        return new TvMazeService(httpClient, logger);
    }

    private static TvMazeService CreateService(HttpStatusCode statusCode)
    {
        var response = new HttpResponseMessage(statusCode);
        
        var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
        {
          BaseAddress = new Uri("https://api.tvmaze.com")
        };

        var logger = NullLogger<TvMazeService>.Instance;
        return new TvMazeService(httpClient, logger);
    }

    [TestMethod]
    public async Task GetTvShowsByPageAsync_WhenApiRespondsWithData_ReturnsShows()
    {
        // Arrange
        var json = JsonSerializer.Serialize(new []
        {
            new TvShow {Id = 1, Name = "Breaking Bad", Language = "English", Status = "Ended"},
            new TvShow {Id = 2, Name = "Better Call Saul", Language = "English", Status = "Ended"}
        });
        var service = CreateService(json);

        // Act
        var result = await service.GetTvShowsByPageAsync(1);

        // Assert
        Assert.HasCount(2, result);
    }

    [TestMethod]
    public async Task GetTvShowsByPageAsync_WhenApiRespondsWithNull_ReturnsEmptyArray()
    {
        // Arrange
        var service = CreateService("null");

        // Act
        var result = await service.GetTvShowsByPageAsync(1);

        // Assert
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public async Task GetTvShowsByPageAsync_WhenApiFails_ReturnsEmptyArray()
    {
        // Arrange
        var service = CreateService(HttpStatusCode.InternalServerError);

        // Act
        var result = await service.GetTvShowsByPageAsync(1);

        // Assert
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public async Task GetTvShowDetailsAsync_WhenApiRespondsWithData_ReturnsShow()
    {
        // Arrange
        var json = JsonSerializer.Serialize(new TvShow
        {
            Name = "Breaking Bad",
            Language = "English",
            Status = "Ended"
        });
        var service = CreateService(json);

        // Act
        var result = await service.GetTvShowDetailsAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Breaking Bad", result.Name);
    }

    [TestMethod]
    public async Task GetTvShowDetailsAsync_WhenApiFails_ReturnsNull()
    {
        // Arrange
        var service = CreateService(HttpStatusCode.InternalServerError);

        // Act
        var result = await service.GetTvShowDetailsAsync(1);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetTvShowEpisodesAsync_WhenApiRespondsWithData_ReturnsEpisodes()
    {
        // Arrange
        var json = JsonSerializer.Serialize(new []
        {
            new TvShowEpisode {Id = 1, Name = "Ozymandias", Season = 5, Number = 14},
            new TvShowEpisode {Id = 2, Name = "Fly", Season = 3, Number = 10}
        });
        var service = CreateService(json);

        // Act
        var result = await service.GetTvShowEpisodesAsync(1);

        // Assert
        Assert.HasCount(2, result);
    }

    [TestMethod]
    public async Task SearchTvShowsAsync_WhenApiRespondsWithData_ReturnsShows()
    {
        // Arrange
        var show1 = new TvShow
        {
            Id = 1, 
            Name = "Breaking Bad", 
            Language = "English", 
            Status = "Ended",
            Image = new TvShowImage {Medium = "http://img.com/1.jpg"}
        };

        var show2 = new TvShow
        {
            Id = 2, 
            Name = "Breaking Bad Extras", 
            Language = "English", 
            Status = "Ended",
            Image = new TvShowImage {Medium = "http://img.com/2.jpg"}
        };

        var json = JsonSerializer.Serialize(new []
        {
           new WrappedTvShow {Score = 9, Show = show1},  
           new WrappedTvShow {Score = 8, Show = show2}
        });
        var service = CreateService(json);

        // Act
        var result = await service.SearchTvShowsAsync("Breaking Bad");

        // Assert
        Assert.HasCount(2, result);
    }
}
