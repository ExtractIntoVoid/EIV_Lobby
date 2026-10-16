using EIV_LobbyData;
using EIV_LobbyData.Socket;
using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Managers;
using ModdableWebServer.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="FriendAction"/>.
/// </summary>
public class FriendActionProcessor : IProcessor
{
    /// <inheritdoc/>
    public bool Process([NotNull] TicketModel ticket, [NotNull] IWSSession session, ICustomMessage message)
    {
        if (!JsonSerializerConvert.TryGetTypeInfo<ClientResponse>(out var typeInfo))
        {
            return false;
        }

        if (message is not FriendAction friendAction)
        {
            return false;
        }

        ResultCode code;
        if (friendAction.IsRequest)
        {
            if (friendAction.Remove)
            {
                code = UserManager.RemoveFriendRequest(ticket.Id, friendAction.Id);
            }
            else
            {
                code = UserManager.SendFriendRequest(ticket.Id, friendAction.Id);
            }
        }
        else
        {
            if (friendAction.Remove)
            {
                code = UserManager.RemoveFriend(ticket.Id, friendAction.Id);
            }
            else
            {
                code = UserManager.AddFriend(ticket.Id, friendAction.Id);
            }
        }

        ClientResponse response = new(code == ResultCode.Success, (uint)ResultCode.Success, code.GetMessage());
        string data = JsonSerializer.Serialize(response, typeInfo);
        session.SendText(data);

        return true;
    }
}
