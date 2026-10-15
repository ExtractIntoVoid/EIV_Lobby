namespace LobbyLib.INI;

/// <summary>
/// Represent a federation configuration.
/// </summary>
public class FederationConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether federation is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public int Port { get; set; }
}
