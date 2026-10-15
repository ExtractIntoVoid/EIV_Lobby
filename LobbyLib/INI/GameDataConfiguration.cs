namespace LobbyLib.INI;

/// <summary>
/// Represent a game data configuration.
/// </summary>
public class GameDataConfiguration
{
    /// <summary>
    /// Gets or sets the database type.
    /// </summary>
    public uint Database { get; set; }

    /// <summary>
    /// Gets or sets the default inventory.
    /// </summary>
    public string DefaultInventoryName { get; set; } = "DefaultInventory";

    /// <summary>
    /// Gets or sets the default origin.
    /// </summary>
    public string DefaultOrigin { get; set; } = "DefaultOrigin";

    /// <summary>
    /// Gets or sets the default stash.
    /// </summary>
    public string DefaultStashName { get; set; } = "DefaultStash";
}
