using System.Collections.ObjectModel;

namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a simple aciont to add friends from the platform.
/// </summary>
/// <param name="Friends">The friends.</param>
public record class FriendAddPlatform(Collection<FriendPlatformInfo> Friends) : ICustomMessage;
