namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a search action for friends.
/// </summary>
/// <param name="Search">The parameter to serach.</param>
public record class FriendSearchRequest(string Search) : ICustomMessage;
