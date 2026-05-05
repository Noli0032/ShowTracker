using System.Net.Http.Json;
using System.Text.Json;
using MyProject.Models;

namespace MyProject.Services;

public sealed class TvMazeService : ITvMazeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TvMazeService> _logger;
    public TvMazeService (HttpClient httpClient, ILogger<TvMazeService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TvShow[]> GetTvShowsByPageAsync(int pageNumber)
    {
        try
        {
            TvShow[]? tvShows = await _httpClient.GetFromJsonAsync<TvShow[]>(
                $"shows?page={pageNumber}",
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            return tvShows ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError("Error when fetching tv shows: {Error}", ex);
        }

        return [];
    }

    public async Task<TvShow?> GetTvShowDetails(int id)
    {
        try
        {
            TvShow? tvShow = await _httpClient.GetFromJsonAsync<TvShow>(
                $"shows/{id}",
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return tvShow;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error when fetching specific tv show details: {error}", ex);
        }

        return null;
    }
}