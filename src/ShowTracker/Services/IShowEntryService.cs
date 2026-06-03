using ShowTracker.Models.UserShow;

namespace ShowTracker.Services;
public interface IShowEntryService
{
    Task UpsertShowEntryAsync(string userId, int tvMazeShowId, ShowStatus showStatus);
    Task<ShowStatus?> GetShowStatusAsync(string userId, int tvMazeShowId);
    Task RemoveUserEntryAsync(string UserId, int tvMazeShowId);
    Task<List<UserShowEntry>>GetWatchlistAsync(string userId);
    Task<List<UserShowEntry>> GetWatchedAsync(string userId);
}