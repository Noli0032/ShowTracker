using MyProject.Models.UserShow;

namespace MyProject.Services;
public interface IShowEntryService
{
    Task AddToWatchlist(string userId, int tvMazeShowId);
    Task<bool> IsInWatchlist(string userID, int tvMazeShowId);
    Task RemoveFromWatchlist(string UserId, int tvMazeShowId);
    Task<List<UserShowEntry>>GetWatchListAsync(string userId);
}