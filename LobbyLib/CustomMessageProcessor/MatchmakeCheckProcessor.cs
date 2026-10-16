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
public class MatchmakeCheckProcessor : IProcessor
{
    /// <inheritdoc/>
    public bool Process([NotNull] TicketModel ticket, [NotNull] IWSSession session, ICustomMessage message)
    {
        if (!JsonSerializerConvert.TryGetTypeInfo<ClientResponse>(out var typeInfo))
        {
            return false;
        }

        if (message is not MatchmakeCheck matchmakeCheck)
        {
            return false;
        }

        ResultCode code = ResultCode.Unknown;

        ClientResponse response = new(code == ResultCode.Success, (uint)ResultCode.Success, code.GetMessage());
        string data = JsonSerializer.Serialize(response, typeInfo);
        session.SendText(data);

        return true;
    }
}
