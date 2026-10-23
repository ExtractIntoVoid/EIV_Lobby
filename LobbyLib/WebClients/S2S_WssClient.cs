using NetCoreServer;
using System.Net;

namespace LobbyLib.WebClients;

/// <summary>
/// Represent a server to server websocket client.
/// </summary>
public class S2S_WssClient : WssClient, IS2SClient
{
    /// <summary>
    /// Occurs when an event with a string identifier and associated byte array data is received.
    /// </summary>
#pragma warning disable CA1003 // Use generic event handler instances
    public static event Action<IS2SClient, byte[]>? OnReceivedEvent;
#pragma warning restore CA1003 // Use generic event handler instances

    private readonly string key;

    /// <summary>
    /// Initializes a new instance of the <see cref="S2S_WssClient"/> class.
    /// </summary>
    /// <param name="connectionKey">The connection key.</param>
    /// <param name="context">SSL Context.</param>
    /// <param name="address">The address.</param>
    /// <param name="port">The port.</param>
    public S2S_WssClient(string connectionKey, SslContext context, string address, int port)
        : base(context, address, port)
    {
        key = connectionKey;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="S2S_WssClient"/> class.
    /// </summary>
    /// <param name="connectionKey">The connection key.</param>
    /// <param name="context">SSL Context.</param>
    /// <param name="endPoint">DNS endpoint.</param>
    public S2S_WssClient(string connectionKey, SslContext context, DnsEndPoint endPoint)
        : base(context, endPoint)
    {
        key = connectionKey;
    }

    /// <inheritdoc/>
    public string Key => key;

    /// <inheritdoc/>
    public override void OnWsConnecting(HttpRequest request)
    {
        if (request == null)
        {
            return;
        }

        request.SetBegin("GET", "/Socket/S2S");
        request.SetHeader("Host", Address);
        request.SetHeader("Origin", Address);
        request.SetHeader("S2SKey", key);
        request.SetHeader("Upgrade", "websocket");
        request.SetHeader("Connection", "Upgrade");
        request.SetHeader("Sec-WebSocket-Key", Convert.ToBase64String(WsNonce));
        request.SetHeader("Sec-WebSocket-Protocol", "s2s");
        request.SetHeader("Sec-WebSocket-Version", "13");
        request.SetBody();
    }

    /// <inheritdoc/>
    public override void OnWsReceived(byte[] buffer, long offset, long size)
    {
        buffer = [.. buffer.Skip((int)offset).Take((int)size)];
        OnReceivedEvent?.Invoke(this, buffer);
    }
}
