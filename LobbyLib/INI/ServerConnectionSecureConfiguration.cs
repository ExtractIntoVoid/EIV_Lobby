using IniParser;
using System.Net;
using System.Security.Authentication;

namespace LobbyLib.INI;

/// <summary>
/// Represent a secure configuration for the <see cref="ServerConnection"/>.
/// </summary>
public class ServerConnectionSecureConfiguration
{
    /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/>
    public static bool TryParse(string s, out ServerConnectionSecureConfiguration connection)
    {
        connection = new();

        if (string.IsNullOrEmpty(s) || !s.Contains(':', StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        string[] splitted = s.Split(':');
        if (splitted.Length != 2)
        {
            return false;
        }

        if (!SslProtocols.TryParse(splitted[0], out SslProtocols protocol))
        {
            return false;
        }

        connection.Protocol = protocol;

        if (!bool.TryParse(splitted[1], out bool verifyCert))
        {
            return false;
        }

        connection.VerifyCertificate = verifyCert;
        return true;
    }

    /// <summary>
    /// Gets or sets the protocol.
    /// </summary>
    public SslProtocols Protocol { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether verify certificate.
    /// </summary>
    public bool VerifyCertificate { get; set; } = true;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Protocol}:{VerifyCertificate}";
    }
}
