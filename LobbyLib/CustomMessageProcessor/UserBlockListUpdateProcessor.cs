using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Managers;
using LobbyLib.Models;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="UserBlockListUpdate"/>.
/// </summary>
public class UserBlockListUpdateProcessor : BaseProcessor<UserBlockListUpdate>
{
    /// <inheritdoc/>
    protected override bool OnProcess([NotNull] UserBlockListUpdate message)
    {
        UserData? myUser = UserManager.GetUser(Ticket);

        if (myUser == null)
        {
            return SendResponse(ResultCode.NoAccountFound);
        }

        if (message.BlockAllChat.HasValue)
        {
            myUser.BlockList.BlockAllChat = message.BlockAllChat.Value;
        }

        if (message.BlockAllInvite.HasValue)
        {
            myUser.BlockList.BlockAllInvite = message.BlockAllInvite.Value;
        }

        if (message.Id.HasValue && message.IsAdd.HasValue && message.UpdateType != UserBlockListUpdateType.None)
        {
            Guid id = message.Id.Value;
            bool isAdd = message.IsAdd.Value;

            if (message.UpdateType.HasFlag(UserBlockListUpdateType.FriendInvite))
            {
                if (isAdd)
                {
                    myUser.BlockList.FriendInviteBlocks.Add(id);
                }
                else
                {
                    myUser.BlockList.FriendInviteBlocks.Remove(id);
                }
            }

            if (message.UpdateType.HasFlag(UserBlockListUpdateType.GroupInvite))
            {
                if (isAdd)
                {
                    myUser.BlockList.GroupInviteBlocks.Add(id);
                }
                else
                {
                    myUser.BlockList.GroupInviteBlocks.Remove(id);
                }
            }

            if (message.UpdateType.HasFlag(UserBlockListUpdateType.Chat))
            {
                if (isAdd)
                {
                    myUser.BlockList.ChatBlocks.Add(id);
                }
                else
                {
                    myUser.BlockList.ChatBlocks.Remove(id);
                }
            }
        }

        UserManager.SaveUser(myUser);

        return SendResponse(ResultCode.Success);
    }
}
