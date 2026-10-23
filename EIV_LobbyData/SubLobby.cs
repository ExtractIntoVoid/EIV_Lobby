namespace EIV_LobbyData;

/// <summary>
/// Represent a sub lobby information.
/// </summary>
/// <param name="Address">The address to connect to.</param>
/// <param name="PlayerNumbers">The current player numbers in the lobby.</param>
/// <param name="MaxPlayerNumbers">The max player number the lobby can handle.</param>
public record class SubLobby(string Address, int PlayerNumbers, int MaxPlayerNumbers);