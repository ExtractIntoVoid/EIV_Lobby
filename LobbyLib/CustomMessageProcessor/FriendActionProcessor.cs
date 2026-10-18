using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Managers;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="FriendAction"/>.
/// </summary>
public class FriendActionProcessor : BaseProcessor<FriendAction>
{
    /// <inheritdoc/>
    protected override bool OnProcess([NotNull] FriendAction message)
    {
        Guid id = Ticket.Id;
        Guid messageId = message.Id;
        ResultCode code;
        if (message.IsRequest)
        {
            if (message.Remove)
            {
                code = UserManager.RemoveFriendRequest(id, messageId);
            }
            else
            {
                code = UserManager.SendFriendRequest(id, messageId);
            }
        }
        else
        {
            if (message.Remove)
            {
                code = UserManager.RemoveFriend(id, messageId);
            }
            else
            {
                code = UserManager.AddFriend(id, messageId);
            }
        }

        return SendResponse(code);
    }
}
