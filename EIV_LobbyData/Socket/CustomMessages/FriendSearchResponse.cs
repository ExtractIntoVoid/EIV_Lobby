using System.Collections.ObjectModel;

namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a response of <see cref="FriendSearchRequest"/>.
/// </summary>
/// <param name="Friends">The found friends.</param>
public record class FriendSearchResponse(Collection<Guid> Friends) : ICustomMessage;
