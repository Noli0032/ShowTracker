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
    public void Sanitize_StripsLinksButKeepsLinkText()
    {
        var result = HtmlSummaryHelper.Sanitize("<p><a href=\"https://example.com\">link</a></p>");

        Assert.AreEqual("<p>link</p>", result);
    }

    [TestMethod]
    public void Sanitize_KeepsLineBreaksAndEmphasis()
    {
        var result = HtmlSummaryHelper.Sanitize("<p>One<br>two <em>three</em> <strong>four</strong></p>");

        Assert.Contains("<br>", result);
        Assert.Contains("<em>three</em>", result);
        Assert.Contains("<strong>four</strong>", result);
    }

    [TestMethod]
    [DataRow("<img src=x onerror=\"alert(1)\">")]
    [DataRow("<svg onload=\"alert(1)\"></svg>")]
    [DataRow("<iframe src=\"https://example.com\"></iframe>")]
    [DataRow("<style>body { display: none }</style>")]
    [DataRow("<scr<script>ipt>alert(1)</scr</script>ipt>")]
    public void Sanitize_RemovesDangerousMarkup(string html)
    {
        var result = HtmlSummaryHelper.Sanitize(html);

        Assert.DoesNotContain("<img", result);
        Assert.DoesNotContain("<svg", result);
        Assert.DoesNotContain("<iframe", result);
        Assert.DoesNotContain("<style", result);
        Assert.DoesNotContain("<script", result);
        Assert.DoesNotContain("onerror", result);
        Assert.DoesNotContain("onload", result);
    }

    [TestMethod]
    public void Sanitize_NullOrWhitespace_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, HtmlSummaryHelper.Sanitize(null));
        Assert.AreEqual(string.Empty, HtmlSummaryHelper.Sanitize("  "));
    }
}
