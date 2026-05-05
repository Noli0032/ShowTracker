using MyProject.Models;
namespace MyProject.Services;

public interface ITvMazeService
{
    Task<TvShow[]> GetWrapperTvShowsByPageAsync(int pageNumber);
}