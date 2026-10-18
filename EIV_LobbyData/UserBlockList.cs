using System.Collections.ObjectModel;

namespace EIV_LobbyData;

/// <summary>
/// Represent a block list.
/// </summary>
/// <param name="FriendInviteBlocks">The friend invites to be blocked.</param>
/// <param name="GroupInviteBlocks">The group invites to be blocked.</param>
/// <param name="ChatBlocks">The ids to block chat communication.</param>
public record class UserBlockList(
    Collection<Guid> FriendInviteBlocks,
    Collection<Guid> GroupInviteBlocks,
    Collection<Guid> ChatBlocks)
{
    /// <summary>
    /// Gets or sets a value indicating whether block all invite.
    /// </summary>
    public bool BlockAllInvite { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether block all chat communication.
    /// </summary>
    public bool BlockAllChat { get; set; }
}