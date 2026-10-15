namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a friend platform information.
/// </summary>
/// <param name="PlatformType">The platform type.</param>
/// <param name="PlatformName">The name on the platform.</param>
/// <param name="PlatformId">The id on the platform.</param>
public record class FriendPlatformInfo(string PlatformType, string PlatformName, string PlatformId);
