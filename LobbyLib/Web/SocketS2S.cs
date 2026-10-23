using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Interfaces;
using ModdableWebServer.Senders;

namespace LobbyLib.Web;

/// <summary>
/// Represent a Lobby Web communication.
/// </summary>
internal partial class LobbyWeb
{
    /// <summary>
    /// Gets a ticket to session dictionary.
    /// </summary>
    public static Dictionary<string, IWSSession> S2SUsers { get; } = [];

    /// <summary>
    /// <see cref="SocketS2S(WebSocketSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
#pragma warning disable CA2243 // Attribute string literals should parse correctly
    [WS("/Socket/S2S", WebSocketMethodListen.All)]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    public static void SocketS2S(WebSocketSender sender)
    {
        IWSSession session = (IWSSession)sender.Session;

        if (!sender.Headers.TryGetValue("S2SKey", out string? key))
        {
            session.SendClose(401, "S2SKey is not found!");
            return;
        }

        switch (sender.CurrentMethod)
        {
            case WebSocketMethodListen.Connected:
                S2SUsers.Add(key, session);
                return;
            case WebSocketMethodListen.Disconnected:
                S2SUsers.Remove(key);
                return;
            case WebSocketMethodListen.Received:
                {
                    /*
                    if (!JsonSerializerConvert.TryGetTypeInfo<ClientMessage>(out var typeInfo))
                    {
                        return;
                    }

#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        ClientMessage? message = JsonSerializer.Deserialize(sender.Buffer, typeInfo);
                        if (message == null)
                        {
                            return;
                        }

                        ProcessorStorage.Process(ticketstruct, session, message.Message);
                    }
                    catch (Exception)
                    {
                        session.SendClose(401, "Cannot Deserialize Message!");
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                    */
                }

                return;
            case WebSocketMethodListen.Close:
                return;
            default:
                return;
        }
    }
}
