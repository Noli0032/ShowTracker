using ShowTracker.ViewModels;

namespace ShowTracker.Services;

public interface IHomeService
{
    Task<HomePageViewModel> GetHomePageAsync();
}
