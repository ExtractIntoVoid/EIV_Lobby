namespace LobbyLib.INI;

/// <summary>
/// Represent ssl configuration.
/// </summary>
public class SSLConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether ssl is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the pfx password.
    /// </summary>
    public string PfxPasword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pfx path.
    /// </summary>
    public string PfxPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the protocol.
    /// </summary>
    public System.Security.Authentication.SslProtocols Protocol { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether verify certificate.
    /// </summary>
    public bool VerifyCertificate { get; set; } = true;
}
