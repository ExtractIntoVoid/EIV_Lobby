namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent an update enum flag for user block list.
/// </summary>
[Flags]
public enum UserBlockListUpdateType
{
    /// <summary>
    /// Do not update list.
    /// </summary>
    None,

    /// <summary>
    /// Update friends invite list.
    /// </summary>
    FriendInvite = 1 << 1,

    /// <summary>
    /// Update group invite list.
    /// </summary>
    GroupInvite = 1 << 2,

    /// <summary>
    /// Update chat list.
    /// </summary>
    Chat = 1 << 3,
}
