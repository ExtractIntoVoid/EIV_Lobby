namespace EIV_LobbyData;

/// <summary>
/// Represent a lobby related information.
/// </summary>
/// <param name="Name">The name of your lobby server.</param>
/// <param name="Description">The short description to display in the lobby selection.</param>
/// <param name="LongDescription">The long description, can contain BBCode.</param>
/// <param name="PlayerNumbers">The current player numbers in the lobby.</param>
/// <param name="MaxPlayerNumbers">The max player number the lobby can handle.</param>
public record class LobbyInfo(string Name, string Description, string LongDescription, int PlayerNumbers, int MaxPlayerNumbers);