namespace ShowTracker.Models;

public record WrappedTvShow
{
    public required float Score {get; init;}
    public required TvShow Show {get; init;}
}