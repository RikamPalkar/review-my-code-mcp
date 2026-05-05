using McpCodeReviewServer.Models;

namespace McpCodeReviewServer.Services;

/// <summary>
/// Calculates review quality score from findings.
/// </summary>
public sealed class ReviewScorer : IReviewScorer
{
    /// <inheritdoc/>
    public int CalculateScore(IReadOnlyCollection<ReviewIssue> issues)
    {
        var score = 10;

        foreach (var issue in issues)
        {
            score -= issue.Severity switch
            {
                "critical" => 3,
                "warning" => 2,
                _ => 1
            };
        }

        return Math.Clamp(score, 0, 10);
    }
}
