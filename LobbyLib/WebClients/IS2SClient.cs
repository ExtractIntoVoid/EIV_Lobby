using NetCoreServer;

namespace LobbyLib.WebClients;

/// <summary>
/// An interface for server to server clients.
/// </summary>
public interface IS2SClient
{
    /// <summary>
    /// Gets the connection key.
    /// </summary>
    public string Key { get; }

    /// <inheritdoc cref="WsClient.Close(int)"/>
    public bool Close(int status);

    /// <inheritdoc cref="WsClient.SendText(string)"/>
    public long SendText(string text);

    /// <inheritdoc cref="WsClient.SendText(byte[])"/>
    public long SendText(byte[] buffer);
}
