using EIV_LobbyData;
using LobbyLib.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.Managers;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Provides user management.
/// </summary>
public static class UserManager
{
    /// <summary>
    /// Gets the user from ticket.
    /// </summary>
    /// <param name="ticket">The ticket.</param>
    /// <returns>The user.</returns>
    public static UserData? GetUser([NotNull] TicketModel ticket)
    {
        return MainControl.Database.GetUserData(ticket.Id);
    }

    /// <summary>
    /// Saves user.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void SaveUser(UserData user)
    {
        MainControl.Database.SaveUserData(user);
    }

    /// <summary>
    /// Gets the user from id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>The user.</returns>
    public static UserData? GetUser(Guid id)
    {
        return MainControl.Database.GetUserData(id);
    }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <returns>The users.</returns>
    public static Collection<UserData> GetUsers()
    {
        return MainControl.Database.GetUserDatas();
    }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="predicate">The selected user.</param>
    /// <returns>The users.</returns>
    public static Collection<UserData> GetUsers(Func<UserData, bool> predicate)
    {
        return new([..GetUsers().Where(predicate)]);
    }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="predicate">The selected user.</param>
    /// <returns>The users.</returns>
    public static IEnumerable<UserData> GetUsersWhere(Func<UserData, bool> predicate)
    {
        return GetUsers().Where(predicate);
    }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="predicate">The selected user.</param>
    /// <returns>The users.</returns>
    public static Collection<Guid> GetUsersGuid(Func<UserData, bool> predicate)
    {
        return new([.. GetUsers().Where(predicate).Select(x => x.Id)]);
    }

    public static ResultCode SendFriendRequest(Guid id, Guid friendId)
    {
        if (!TryGetUserAndFriend(id, friendId, out var user, out var friend))
        {
            return ResultCode.NoAccountFound;
        }

        if (IsFriend(id, friendId))
        {
            return ResultCode.AlreadyFriend;
        }

        if (user.BlockList.BlockAllInvite || friend.BlockList.BlockAllInvite)
        {
            return ResultCode.IsInBlockList;
        }

        if (user.BlockList.FriendInviteBlocks.Contains(friendId) || friend.BlockList.FriendInviteBlocks.Contains(id))
        {
            return ResultCode.IsInBlockList;
        }

        user.FriendRequests.Add(friendId);
        friend.FriendRequests.Add(id);

        SaveUser(user);
        SaveUser(friend);

        return ResultCode.Success;
    }

    public static ResultCode RemoveFriendRequest(Guid id, Guid friendId)
    {
        if (!TryGetUserAndFriend(id, friendId, out var user, out var friend))
        {
            return ResultCode.NoAccountFound;
        }

        if (IsFriend(id, friendId))
        {
            return ResultCode.AlreadyFriend;
        }

        if (IsInFriendRequest(id, friendId))
        {
            return ResultCode.NotFoundInRequest;
        }

        user.FriendsIds.Remove(friendId);
        friend.FriendsIds.Remove(friendId);

        SaveUser(user);
        SaveUser(friend);

        return ResultCode.Success;
    }

    public static ResultCode AddFriend(Guid id, Guid friendId)
    {
        if (!TryGetUserAndFriend(id, friendId, out var user, out var friend))
        {
            return ResultCode.NoAccountFound;
        }

        if (IsFriend(id, friendId))
        {
            return ResultCode.AlreadyFriend;
        }

        if (IsInFriendRequest(id, friendId))
        {
            return ResultCode.NotFoundInRequest;
        }

        if (user.BlockList.BlockAllInvite || friend.BlockList.BlockAllInvite)
        {
            return ResultCode.IsInBlockList;
        }

        if (user.BlockList.FriendInviteBlocks.Contains(friendId) || friend.BlockList.FriendInviteBlocks.Contains(id))
        {
            return ResultCode.IsInBlockList;
        }

        user.FriendsIds.Add(friendId);
        friend.FriendsIds.Add(friendId);

        SaveUser(user);
        SaveUser(friend);

        return ResultCode.Success;
    }

    public static ResultCode RemoveFriend(Guid id, Guid friendId)
    {
        if (!TryGetUserAndFriend(id, friendId, out var user, out var friend))
        {
            return ResultCode.NoAccountFound;
        }

        if (!IsFriend(id, friendId))
        {
            return ResultCode.NotFoundInFriends;
        }

        user.FriendsIds.Remove(friendId);
        friend.FriendsIds.Remove(friendId);

        SaveUser(user);
        SaveUser(friend);

        return ResultCode.Success;
    }

    public static bool IsInFriendRequest(Guid id, Guid friendId)
    {
        var user = GetUser(id);
        if (user == null)
        {
            return false;
        }

        return user.FriendRequests.Contains(friendId);
    }

    public static bool IsFriend(Guid id, Guid friendId)
    {
        var user = GetUser(id);
        if (user == null)
        {
            return false;
        }

        return user.FriendsIds.Contains(friendId);
    }

    public static bool TryGetUserAndFriend(Guid id, Guid friendId, [NotNullWhen(true)] out UserData? user, [NotNullWhen(true)] out UserData? friend)
    {
        user = GetUser(id);
        friend = GetUser(friendId);
        return user != null && friend != null;
    }
}
