using ShowTracker.Helpers;

namespace ShowTracker.Tests.Helpers;

[TestClass]
public class HtmlSummaryHelperTests
{
    [TestMethod]
    public void Sanitize_KeepsAllowedFormatting()
    {
        var result = HtmlSummaryHelper.Sanitize("<p>A <b>bold</b> and <i>italic</i> story.</p>");

        Assert.AreEqual("<p>A <b>bold</b> and <i>italic</i> story.</p>", result);
    }

    [TestMethod]
    public void Sanitize_RemovesScriptTags()
    {
        var result = HtmlSummaryHelper.Sanitize("<p>Hi</p><script>alert(1)</script>");

        Assert.DoesNotContain("script", result);
        Assert.Contains("Hi", result);
    }

    [TestMethod]
    public void Sanitize_RemovesEventHandlersAndLinks()
    {
        var result = HtmlSummaryHelper.Sanitize("<p onclick=\"x()\"><a href=\"javascript:x()\">link</a></p>");

        Assert.DoesNotContain("onclick", result);
        Assert.DoesNotContain("javascript", result);
    }

    [TestMethod]
    public void Sanitize_NullOrWhitespace_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, HtmlSummaryHelper.Sanitize(null));
        Assert.AreEqual(string.Empty, HtmlSummaryHelper.Sanitize("  "));
    }
}
