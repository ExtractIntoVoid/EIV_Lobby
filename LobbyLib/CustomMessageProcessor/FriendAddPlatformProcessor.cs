using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Managers;
using LobbyLib.Models;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="FriendAddPlatform"/>.
/// </summary>
public class FriendAddPlatformProcessor : BaseProcessor<FriendAddPlatform>
{
    /// <inheritdoc/>
    protected override bool OnProcess([NotNull] FriendAddPlatform message)
    {
        UserData? myUser = UserManager.GetUser(Ticket);

        if (myUser == null)
        {
            return SendResponse(ResultCode.NoAccountFound);
        }

        foreach (FriendPlatformInfo platformInfo in message.Friends)
        {
            UserData? foundFriend = UserManager.GetUsersWhere(x =>
                x.Name == platformInfo.PlatformName ||
                x.CustomId == $"{platformInfo.PlatformId}@{platformInfo.PlatformType}")
                .FirstOrDefault();

            if (foundFriend == null)
            {
                continue;
            }

            if (!UserManager.IsFriend(myUser.Id, foundFriend.Id))
            {
                myUser.FriendsIds.Add(foundFriend.Id);
                myUser.FriendRequests.Remove(foundFriend.Id);
                UserManager.SaveUser(myUser);
            }

            if (!UserManager.IsFriend(foundFriend.Id, myUser.Id))
            {
                foundFriend.FriendsIds.Add(myUser.Id);
                foundFriend.FriendRequests.Remove(myUser.Id);
                UserManager.SaveUser(foundFriend);
            }
        }

        return SendResponse(ResultCode.Success);
    }
}
