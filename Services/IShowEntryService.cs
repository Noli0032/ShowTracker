namespace MyProject.Services;

public interface IShowEntryService
{
    Task AddToWatchList(string userId, int tvMazeShowId);
    Task<bool> IsInWatchList(string userID, int tvMazeShowId);
}