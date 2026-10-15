namespace LobbyLib.INI;

/// <summary>
/// Represent a lobby configuration.
/// </summary>
public class LobbyConfiguration
{
    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; } = "127.0.0.1";

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public int Port { get; set; } = 7777;

    /// <summary>
    /// Gets or sets the lobby name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lobby description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lobby long description file name.
    /// </summary>
    public string LongDescriptionFileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the accepted version range.
    /// </summary>
    public string VersionRange { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the max players.
    /// </summary>
    public int MaxPlayers { get; set; }

    /// <summary>
    /// Reads long description.
    /// </summary>
    /// <returns>The long description.</returns>
    public string ReadLongDescription()
    {
        if (string.IsNullOrEmpty(LongDescriptionFileName))
        {
            return string.Empty;
        }

        string filename = Path.Combine(Directory.GetCurrentDirectory(), LongDescriptionFileName);
        if (!File.Exists(filename))
        {
            return string.Empty;
        }

        return File.ReadAllText(filename);
    }
}
