namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a friend add/remove action.
/// </summary>
/// <param name="Id">The friend id.</param>
/// <param name="Remove">Whenever to remove or add the friend.</param>
/// <param name="IsRequest">Whenever to send a request if not already a friend.</param>
public record class FriendAction(Guid Id, bool Remove, bool IsRequest) : ICustomMessage;
