using System.ComponentModel.DataAnnotations;

namespace MyProject.Models;

public class TvShow
{
    public int Id {get; set;}
    public required string Name {get; set;}
    public required string Language {get; set;}
    public required List<String> Genres {get; set;}
    public required string Status {get; set;}
    public DateOnly Premiered {get; set;}
    public DateOnly Ended {get; set;}
    public int AvgRating {get; set;}
    public string? Image {get; set;}
    public string? Summary {get; set;}
}