using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Identity.Data;
using MyProject.Models;
using MyProject.Models.UserShow;

namespace MyProject.Services;

public class CachedShowService : ICachedShowService
{
    private readonly ApplicationDbContext _context;
    private readonly ITvMazeService _tvMazeService;
    public CachedShowService(ApplicationDbContext context, ITvMazeService tvMazeService)
    {
        _context = context;
        _tvMazeService = tvMazeService;
    }

    public async Task<CachedShow?> EnsureCachedAsync(int tvMazeShowId)
    {
        if (await _context.CachedShow.AnyAsync(entry => entry.TvMazeShowId == tvMazeShowId))
        {
            // If show is already cached, we return it
            return await _context.CachedShow.SingleOrDefaultAsync(entry => entry.TvMazeShowId == tvMazeShowId);
        }
        else
        {
            TvShow? tvShow = await _tvMazeService.GetTvShowDetails(tvMazeShowId);
            // Return early if tv show could not be fetched
            if (tvShow == null) return null;

            string? imagePath = null;
            if(tvShow.Image?.Medium != null)
            {
                Uri siteUri = new Uri(tvShow.Image.Medium);
                // We only need to store the last two segments of the ImageUrl
                imagePath = siteUri.Segments[^2] + siteUri.Segments[^1];   
            }

            var cachedShow = new CachedShow{TvMazeShowId = tvShow.Id, Name = tvShow.Name, ImageUrl = imagePath, LastSyncedAt = DateTime.UtcNow};
            _context.Add(cachedShow);
            await _context.SaveChangesAsync();
            return cachedShow;
        }
    }
}