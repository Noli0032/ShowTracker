using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
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
    public async Task GetTvShowsByPageAsync_WhenApiRespondsWithNull_ReturnEmptyArray()
    {
        // Arrange
        var service = CreateService("null");

        // Act
        var result = await service.GetTvShowsByPageAsync(1);

        // Assert
        Assert.IsEmpty(result);
    }
}
