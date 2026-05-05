using McpCodeReviewServer.Models;

namespace McpCodeReviewServer.Services;

/// <summary>
/// Defines review scoring behavior.
/// </summary>
public interface IReviewScorer
{
    /// <summary>
    /// Calculates a score for findings.
    /// </summary>
    /// <param name="issues">Review findings.</param>
    /// <returns>Score in range 0-10.</returns>
    int CalculateScore(IReadOnlyCollection<ReviewIssue> issues);
}
