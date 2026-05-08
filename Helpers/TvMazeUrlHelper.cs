namespace MyProject.Helpers;

public static class TvMazeUrlHelper
{
    private const string BaseUrl = "https://static.tvmaze.com/uploads/images/";
    public static string? GetMediumImageUrl(string? path)
        => path != null ? $"{BaseUrl}medium_portrait/{path}" : null;
    
    public static string? GetOriginalImageUrl(string? path)
        => path != null ? $"{BaseUrl}original_portrait/{path}" : null;
}