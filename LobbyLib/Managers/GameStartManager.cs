using EIV_Common;
using LobbyLib.SocketControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

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
            StartGame(ServerPath, string.Empty);
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
        bool UseAsRange = ConfigINI.Read<bool>("Config.ini", "GameServer", "UseAsRange");
        if (!PortsAvailable.Contains(","))
        {
            // it doesnt have ports as a list. might be has 1 value?
            if (!int.TryParse(PortsAvailable, out int res))
            {
                // nope, config fucked.
                return (string.Empty, 0);
            }
            if (!CheckIfPortAvailable(res))
            {
                // port not avlbl. return nothing.
                return (string.Empty, 0);
            }
            else
            {
                StartGame(ServerPath, $"--map={map} --port={res}");
                SockControl.StartServer(res, map);
            }
        }
        // todo: make the ports read when in range and when it has more. thx
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
