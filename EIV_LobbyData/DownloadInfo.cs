namespace EIV_LobbyData;

/// <summary>
/// Represent a download related information.
/// </summary>
/// <param name="Name">The name to download.</param>
/// <param name="Hash">The download hash.</param>
/// <param name="Link">The download link.</param>
public record class DownloadInfo(string Name, string Hash, string Link);