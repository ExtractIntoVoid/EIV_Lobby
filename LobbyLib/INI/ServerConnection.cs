namespace LobbyLib.INI;

/// <summary>
/// Represent a simple <see cref="Address"/> and <see cref="Port"/>.
/// </summary>
public class ServerConnection
{
    /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/>
    public static bool TryParse(string s, out ServerConnection connection)
    {
        connection = new();

        if (string.IsNullOrEmpty(s) || !s.Contains(':', StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        string[] splitted = s.Split(':');
        if (splitted.Length != 4)
        {
            return false;
        }

        connection.Address = splitted[0];
        if (!int.TryParse(splitted[1], out int port))
        {
            return false;
        }

        connection.Port = port;

        if (!bool.TryParse(splitted[2], out bool isSecure))
        {
            return false;
        }

        connection.IsSecure = isSecure;

        if (!bool.TryParse(splitted[3], out bool isSubLobby))
        {
            return false;
        }

        connection.IsSubLobby = isSubLobby;

        connection.ConnectionKey = splitted[4];
        return true;
    }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the connection is secure.
    /// </summary>
    public bool IsSecure { get; set; }

    /// <summary>
    /// Gets or sets the connection key.
    /// </summary>
    public string ConnectionKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the connection is a sub-lobby.
    /// </summary>
    public bool IsSubLobby { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Address}:{Port}:{IsSecure}:{IsSubLobby}:{ConnectionKey}";
    }
}