using IniParser;
using System.Security.Cryptography;

namespace LobbyLib.INI;

/// <summary>
/// Represent a server-to-server configuration.
/// </summary>
public class Server2ServerConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether s2s is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets the connection to the server.
    /// </summary>
    [GenerateIniComment("Written as: {Address}:{Port}:{IsSecure}:{IsSubLobby}:{ConnectionKey}.")]
#pragma warning disable CA1002 // Do not expose generic lists
    public List<ServerConnection> Connections { get; init; } = [new() { Address = "localhost", Port = 6666 }];
#pragma warning restore CA1002 // Do not expose generic lists

    /// <summary>
    /// Gets or sets the connection key.
    /// </summary>
    [GenerateIniComment("Use random value for connection to the server.")]
    public string ConnectionKey { get; set; } = RandomNumberGenerator.GetHexString(16, true);

    /// <summary>
    /// Gets the secure configuration settings for server connections.
    /// </summary>
    [GenerateIniComment("Key are the Connection Address and port.\nValue are the Protocol and Verify Certificate.")]
    public Dictionary<string, ServerConnectionSecureConfiguration> SecureConfiguration { get; init; } = new()
    {
        // Key are the Address + port.
        ["localhost:6666"] = new()
        {
            Protocol = System.Security.Authentication.SslProtocols.None,
            VerifyCertificate = false,
        },
    };
}
