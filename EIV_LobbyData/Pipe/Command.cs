namespace EIV_LobbyData.Pipe;

/// <summary>
/// Represent a pipe command.
/// </summary>
/// <param name="FullCommandWithArgs">Full command with name and args.</param>
/// <param name="RunAsClient">Whenever to run as client or server.</param>
public record class Command(string FullCommandWithArgs, bool RunAsClient) : IPipeMessage;
