namespace MyProject.Services;

public interface IShowEntryService
{
    Task AddToWatchList(string userId, int tvMazeShowId);
}