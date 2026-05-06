using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Identity.Data;
using MyProject.Models.UserShow;

namespace MyProject.Services;

public class ShowEntryService : IShowEntryService
{
    private readonly ApplicationDbContext _context;

    public ShowEntryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddToWatchlist(string userId, int tvMazeShowId)
    {
        var showEntry = new UserShowEntry{UserId = userId, TvMazeShowId = tvMazeShowId, Status = ShowStatus.Watchlist};
        _context.UserShowEntries.Add(showEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsInWatchlist(string userId, int tvMazeShowId)
    {
        return await _context.UserShowEntries
        .AnyAsync(entry => entry.UserId == userId && entry.TvMazeShowId == tvMazeShowId);
    }

    public async Task RemoveFromWatchlist(string userId, int tvMazeShowId)
    {
        var showEntry = await _context.UserShowEntries.SingleOrDefaultAsync(entry => entry.UserId == userId && entry.TvMazeShowId == tvMazeShowId);
        if(showEntry == null)
        {
            return;
        }
        _context.UserShowEntries.Remove(showEntry);
        await _context.SaveChangesAsync();
    }
}