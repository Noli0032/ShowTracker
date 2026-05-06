namespace MyProject.Models;

public record TvShow
{
    public int Id {get; init;}
    public required string Name {get; init;}
    public required string Language {get; init;}
    public List<String> Genres {get; init;} = [];
    public required string Status {get; init;}
    public string? Premiered {get; init;}
    public string? Ended {get; init;}
    public TvShowRating? Rating {get; init;}
    public TvShowImage? Image {get; init;}
    public string? Summary {get; init;}
}