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

    public async Task AddToWatchList(string userId, int tvMazeShowId)
    {
        var showEntry = new UserShowEntry{UserId = userId, TvMazeShowId = tvMazeShowId, Status = ShowStatus.Watchlist};
        _context.UserShowEntries.Add(showEntry);
        await _context.SaveChangesAsync();
    }
}