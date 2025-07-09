using EIV_Common;
using LobbyLib.SocketControl;
using System.Collections.Generic;
using System.Diagnostics;

namespace LobbyLib.Managers;

internal class GameStartManager
{
    public static List<Process> GameServerProcesses { get; set; } = [];

    public static void ControlInit()
    {
        bool LaunchGameServerInstant = ConfigINI.Read<bool>("Config.ini", "GameServer", "LaunchGameServerInstant");
        string ServerPath = ConfigINI.Read("Config.ini", "GameServer", "ServerPath");
        if (LaunchGameServerInstant && !string.IsNullOrEmpty(ServerPath))
        {
            // no host should disable auto hosting.
            // that means every mod should load then we get a list for maps, then sync and quit.
            StartGame(ServerPath, "--nohost --syncmap --quit");
        }
    }
    public static (string ip, int port) StartGameServer(string map)
    {
        string ServerPath = ConfigINI.Read("Config.ini", "GameServer", "ServerPath");
        if (string.IsNullOrEmpty(ServerPath))
            return (string.Empty, 0);
        if (!File.Exists(ServerPath))
            return (string.Empty, 0);
        string PortsAvailable = ConfigINI.Read("Config.ini", "GameServer", "PortsAvailable");
        if (!PortsAvailable.Contains(','))
        {
            // it doesnt have ports as a list. might be has 1 value?
            if (!int.TryParse(PortsAvailable, out int res))
            {
                // nope, config fucked.
                return (string.Empty, 0);
            }
            if (!CheckIfPortAvailable(res))
            {
                // port not Available. return nothing.
                return (string.Empty, 0);
            }
            else
            {
                StartGame(ServerPath, $"--map={map} --port={res}");
                SockControl.StartServer(res);
                return (ConfigINI.Read("Config.ini", "Lobby", "ServerAddress"), res);
            }
        }
        List<int> Ports = [];
        foreach (var port in PortsAvailable.Split('-'))
        {
            if (port.Contains('-'))
            {
                var split_port = port.Split('-');
                var first_port_str = split_port[0];
                var last_port_str = split_port[1];
                if (!int.TryParse(first_port_str, out int first_port))
                {
                    continue;
                }
                if (!int.TryParse(last_port_str, out int last_port))
                {
                    continue;
                }
                Ports.AddRange(Enumerable.Range(first_port, last_port));
            }

            if (!int.TryParse(port, out int res))
            {
                continue;
            }
            Ports.Add(res);
        }
        foreach (var port in Ports)
        {
            if (!CheckIfPortAvailable(port))
            {
                continue;
            }
            StartGame(ServerPath, $"--map={map} --port={port}");
            SockControl.StartServer(port);
            return (ConfigINI.Read("Config.ini", "Lobby", "ServerAddress"), port);
        }

        return (string.Empty, 0);
    }

    public static bool StartGame(string path, string args)
    {
        Process.EnterDebugMode();
        var proc = Process.Start(new ProcessStartInfo()
        {
            FileName = path,
            Arguments = args
        });
        if (proc != null)
        {
            GameServerProcesses.Add(proc);
            return true;
        }
        return false;
    }

    public static void CheckProcesss()
    {
        List<Process> ToRemove = [];
        foreach (var process in GameServerProcesses)
        {
            if (process.HasExited)
                ToRemove.Add(process);
        }
        foreach (var process in ToRemove)
        {
            GameServerProcesses.Remove(process);
        }
    }

    public static bool CheckIfPortAvailable(int port)
    {
        return !System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any(p => p.Port == port);
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
