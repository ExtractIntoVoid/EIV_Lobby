namespace LobbyLib.INI;

/// <summary>
/// Represent a game server configuration.
/// </summary>
public class GameServerConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether launch server instantly after lobby started.
    /// </summary>
    public bool InstantLaunch { get; set; }

    /// <summary>
    /// Gets or sets the server exe path.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the available ports.
    /// </summary>
    [IniParser.GenerateIniComment(" Use '-' to set range! (ie: 8888,7878-7900)")]
    public string PortsAvailable { get; set; } = string.Empty;
}
