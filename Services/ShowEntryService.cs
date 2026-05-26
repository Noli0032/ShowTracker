using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Identity.Data;
using MyProject.Models.UserShow;

namespace MyProject.Services;

public class ShowEntryService : IShowEntryService
{
    private readonly ApplicationDbContext _context;
    private readonly ICachedShowService _cachedShowService;

    public ShowEntryService(ApplicationDbContext context, ICachedShowService cachedShowService)
    {
        _context = context;
        _cachedShowService = cachedShowService;
    }

    public async Task UpsertShowEntryAsync(string userId, int tvMazeShowId, ShowStatus showStatus)
    {
        CachedShow? cachedShow = await _cachedShowService.EnsureCachedAsync(tvMazeShowId);

        // Something went wrong, show could not be found on TvMaze
        if (cachedShow == null) return;

        // Check if a show entry already exists for this user on this show
        var showEntry = await _context.UserShowEntries
            .SingleOrDefaultAsync(e => e.UserId == userId && e.TvMazeShowId == tvMazeShowId);

        // No show entry exists, create one
        if(showEntry == null)
        {
            showEntry = new UserShowEntry{UserId = userId, TvMazeShowId = tvMazeShowId, Status = showStatus, DateAdded = DateOnly.FromDateTime(DateTime.UtcNow)};
            _context.UserShowEntries.Add(showEntry);
        }
        // If it exists, update the status
        else
        {
            showEntry.Status = showStatus;
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ShowStatus?> GetShowStatusAsync(string userId, int tvMazeShowId)
    {
        var showEntry = await _context.UserShowEntries
        .SingleOrDefaultAsync(entry => entry.UserId == userId && entry.TvMazeShowId == tvMazeShowId);

        return showEntry?.Status;
    }

    public async Task RemoveUserEntryAsync(string userId, int tvMazeShowId)
    {
        var showEntry = await _context.UserShowEntries.SingleOrDefaultAsync(entry => entry.UserId == userId && entry.TvMazeShowId == tvMazeShowId);
        if(showEntry == null)
        {
            return;
        }
        _context.UserShowEntries.Remove(showEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserShowEntry>> GetWatchListAsync(string userId)
    {
        return await _context.UserShowEntries
        .Where(entry => entry.UserId == userId)
        .Include(entry => entry.CachedShow)
        .ToListAsync();
    }
}