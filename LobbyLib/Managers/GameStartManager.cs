using LobbyLib.INI;
using Serilog;
using System.Diagnostics;

namespace LobbyLib.Managers;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class GameStartManager
{
    private static readonly Dictionary<Process, string> GameServerProcesses = [];

    public static void ControlInit()
    {
        string serverPath = MainIni.Instance.GameServer.Path;
        if (MainIni.Instance.GameServer.InstantLaunch && !string.IsNullOrEmpty(serverPath))
        {
            // no host should disable auto hosting.
            // that means every mod should load then we get a list for maps, then sync and quit.
            StartGame(serverPath, "--nohost --syncmap --quit");
        }
    }

    public static void StartGameServer(string map)
    {
        string serverPath = MainIni.Instance.GameServer.Path;
        if (string.IsNullOrEmpty(serverPath))
        {
            return;
        }

        StartGame(serverPath, $"--map={map}");
    }

    public static void StartGame(string path, string args)
    {
        string pipeComId = $"Lobby_{Guid.NewGuid()}";
        PipeCommunication.StartServer(pipeComId);

        args += " --np=" + pipeComId;

        Process process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = path,
                Arguments = args,
                UseShellExecute = !MainIni.Instance.GameServer.Silent && MainIni.Instance.GameServer.NewWindow,
                RedirectStandardError = MainIni.Instance.GameServer.Silent,
                RedirectStandardInput = MainIni.Instance.GameServer.Silent,
                RedirectStandardOutput = MainIni.Instance.GameServer.Silent,
            },
        };

        GameServerProcesses.Add(process, pipeComId);

        process.Start();
    }

    public static void CheckProcesss()
    {
        List<Process> toRemove = [];

        foreach (var process in GameServerProcesses)
        {
            if (process.Key.HasExited)
            {
                toRemove.Add(process.Key);
                PipeCommunication.StopServer(process.Value);
            }
        }

        foreach (var process in toRemove)
        {
            GameServerProcesses.Remove(process);
        }
    }

    public static void StopAll()
    {
        foreach (var item in GameServerProcesses)
        {
            item.Key.Kill();
            item.Key.Close();
            item.Key.Dispose();
            PipeCommunication.StopServer(item.Value);
        }

        GameServerProcesses.Clear();
    }
}
