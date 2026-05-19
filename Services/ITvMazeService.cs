using MyProject.Models;
namespace MyProject.Services;

public interface ITvMazeService
{
    Task<TvShow[]> GetTvShowsByPageAsync(int pageNumber);
    Task<TvShow?> GetTvShowDetails(int id);
    Task<TvShowEpisode[]> GetTvShowEpisodesAsync(int tvMazeShowId);
}