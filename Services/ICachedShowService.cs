namespace MyProject.Services;
using MyProject.Models.UserShow;

public interface ICachedShowService
{
    Task<CachedShow?> EnsureCachedAsync(int tvMazeShowId);
}