namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a friend add/remove action.
/// </summary>
/// <param name="Id">The friend id.</param>
/// <param name="Remove">Whenever to remove or add the friend.</param>
public record class FriendAction(Guid Id, bool Remove) : ICustomMessage;
