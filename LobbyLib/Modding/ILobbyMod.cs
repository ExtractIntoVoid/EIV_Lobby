using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;
using ModdableWebServer;

namespace LobbyLib.Modding;

public interface ILobbyMod
{
    public void Initialize();
    public void ShutDown();
    public bool ClientEvent(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketEnum clientSocketEnum, ClientSocketMessage clientSocketMessage);
}
