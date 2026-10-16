using LobbyLib.INI;
using System.Diagnostics;

namespace LobbyLib.Managers;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class GameStartManager
{
    private static readonly List<Process> GameServerProcesses = [];

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

    public static bool StartGame(string path, string args)
    {
        Process.EnterDebugMode();

        string pipeComId = $"Lobby_{Guid.NewGuid()}";
        PipeCommunication.StartServer(pipeComId);

        args += "--np=" + pipeComId;

        var proc = Process.Start(new ProcessStartInfo()
        {
            FileName = path,
            Arguments = args,
        });

        if (proc == null)
        {
            return false;
        }

        GameServerProcesses.Add(proc);
        return true;
    }

    public static void CheckProcesss()
    {
        List<Process> toRemove = [];

        foreach (var process in GameServerProcesses)
        {
            if (process.HasExited)
            {
                toRemove.Add(process);
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
            item.Kill();
            item.Close();
            item.Dispose();
        }

        GameServerProcesses.Clear();
    }
}
