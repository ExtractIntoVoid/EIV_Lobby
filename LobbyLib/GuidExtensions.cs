using System.Diagnostics.CodeAnalysis;

namespace LobbyLib;

/// <summary>
/// Provides an extension for guid.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Simplyfy guid.
    /// </summary>
    /// <param name="id">The guid.</param>
    /// <returns>The simplified string.</returns>
    public static string Simplyfy([NotNull] this Guid id)
    {
        return id.ToString().Replace('-', '_');
    }
}
