using McpCodeReviewServer.Models;
using McpCodeReviewServer.Rules.Abstractions;

namespace McpCodeReviewServer.Services;

/// <summary>
/// Executes registered rule sets and returns bounded findings.
/// </summary>
public sealed class ReviewAnalyzer : IReviewAnalyzer
{
    private readonly IReadOnlyCollection<ICodeRule> _rules;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewAnalyzer"/> class.
    /// </summary>
    /// <param name="ruleGroups">Rule group providers discovered from DI.</param>
    public ReviewAnalyzer(IEnumerable<IRuleGroupProvider> ruleGroups)
    {
        _rules = ruleGroups.SelectMany(group => group.BuildRules()).ToArray();
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<ReviewIssue> Analyze(string code, int maxIssues)
    {
        var normalizedMax = Math.Max(1, maxIssues);
        var lines = NormalizeLines(code);
        var context = new RuleContext(code, lines);

        var issues = new List<ReviewIssue>();
        foreach (var rule in _rules)
        {
            var issue = rule.Evaluate(context);
            if (issue is null)
            {
                continue;
            }

            issues.Add(issue);
            if (issues.Count >= normalizedMax)
            {
                break;
            }
        }

        return issues;
    }

    private static IReadOnlyList<string> NormalizeLines(string code) =>
        code.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');
}
