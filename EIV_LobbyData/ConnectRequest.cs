namespace EIV_LobbyData;

/// <summary>
/// Represent a "/Connect" request.
/// </summary>
/// <param name="Version">The game version.</param>
/// <param name="PlatformName">The platform name.</param>
/// <param name="PlatformId">The id on the platform.</param>
/// <param name="PlatformUserName">The name on the platform.</param>
/// <param name="AdditionalData">Additional information.</param>
public record class ConnectRequest(string Version, string PlatformName, string PlatformId, string PlatformUserName, string AdditionalData)
{
    /// <summary>
    /// Gets the User Id with specify platform.
    /// </summary>
    public string UserId => $"{PlatformId}@{PlatformUserName}";
}