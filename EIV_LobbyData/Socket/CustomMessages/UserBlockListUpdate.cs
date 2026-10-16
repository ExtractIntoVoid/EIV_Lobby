namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Updates <see cref="UserBlockList"/>.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="IsAdd">Whenever to add into the list.</param>
/// <param name="UpdateType">The update type.</param>
/// <param name="BlockAllInvite">Whenever to update block all invite.</param>
/// <param name="BlockAllChat">Whenever to update block all chat.</param>
public record class UserBlockListUpdate(Guid? Id, bool? IsAdd, UserBlockListUpdateType UpdateType = UserBlockListUpdateType.None, bool? BlockAllInvite = null, bool? BlockAllChat = null) : ICustomMessage;
