using System.Collections.ObjectModel;

namespace EIV_LobbyData.Socket.S2SMessages;

/// <summary>
/// Sync all info to the connected server.
/// </summary>
public class SyncConnected : IS2SMessage
{
    /// <summary>
    /// Gets or sets the sub lobby.
    /// </summary>
    public SubLobby? SubLobby { get; set; }

    /// <summary>
    /// Gets the collection of map names.
    /// </summary>
    public Collection<string> Maps { get; init; } = [];

    /// <summary>
    /// Gets the collection of downloadable content.
    /// </summary>
    public Collection<DownloadInfo> Downloadables { get; init; } = [];
}
