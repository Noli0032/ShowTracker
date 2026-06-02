namespace ShowTracker.Services;
using ShowTracker.Models.UserShow;

public interface ICachedShowService
{
    Task<CachedShow?> EnsureCachedAsync(int tvMazeShowId);
}