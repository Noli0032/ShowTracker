using Ganss.Xss;

namespace ShowTracker.Helpers;

public static class HtmlSummaryHelper
{
    private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        foreach (var tag in new[] { "p", "b", "i", "em", "strong", "br" })
        {
            sanitizer.AllowedTags.Add(tag);
        }
        sanitizer.AllowedAttributes.Clear();
        return sanitizer;
    }

    public static string Sanitize(string? html)
        => string.IsNullOrWhiteSpace(html) ? string.Empty : Sanitizer.Sanitize(html);
}
