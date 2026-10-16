using System.Collections.ObjectModel;

namespace EIV_LobbyData;

/// <summary>
/// Represent a block list.
/// </summary>
/// <param name="FriendInviteBlocks">The friend invites to be blocked.</param>
/// <param name="GroupInviteBlocks">The group invites to be blocked.</param>
/// <param name="ChatBlocks">The ids to block chat communication.</param>
/// <param name="BlockAllInvite">Whenever to block all invite.</param>
/// <param name="BlockAllChat">Whenever to block all chat communication.</param>
public record class UserBlockList(
    Collection<Guid> FriendInviteBlocks,
    Collection<Guid> GroupInviteBlocks,
    Collection<Guid> ChatBlocks,
    bool BlockAllInvite,
    bool BlockAllChat);