namespace McpCodeReviewServer.Rules.Abstractions;

/// <summary>
/// Defines a provider that yields related rules for one category area.
/// </summary>
public interface IRuleGroupProvider
{
    /// <summary>
    /// Builds rule instances for this group.
    /// </summary>
    /// <returns>Rule instances to execute.</returns>
    IReadOnlyCollection<ICodeRule> BuildRules();
}
