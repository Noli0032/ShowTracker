using ShowTracker.Models;
namespace ShowTracker.Services;

public interface ITvMazeService
{
    Task<TvShow[]> GetTvShowsByPageAsync(int pageNumber);
    Task<TvShow?> GetTvShowDetailsAsync(int id);
    Task<TvShowEpisode[]> GetTvShowEpisodesAsync(int tvMazeShowId);
    Task<TvShow[]> SearchTvShowsAsync(string query);
}