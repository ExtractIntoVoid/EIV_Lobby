using EIV_Common;
using LobbyLib.Database;
using LobbyLib.Modding;
using LobbyLib.Web;
using System.Diagnostics;

namespace LobbyLib;

public class MainControl
{
    public static bool IsAlreadyQuited = false;
    public static string IP = "https://127.0.0.1:7777";
    public static string ip_port = "127.0.0.1:7777";
    public static IDatabase Database = new EmptyDatabase();
    public static List<Process> GameServerProcesses = [];

    /// <summary>
    /// Init the Server
    /// </summary>
    public static bool InitAll()
    {
        // ini check
        if (!File.Exists("Config.ini"))
        {
            File.WriteAllText("Config.ini", LobbyLib_Res.Config);
        }

        bool ssl = ConfigINI.Read<bool>("Config.ini", "Lobby", "SSL");
        string Ip = ConfigINI.Read("Config.ini", "Lobby", "ServerAddress");
        ushort port = ConfigINI.Read<ushort>("Config.ini", "Lobby", "ServerPort");
        if (ssl)
        {
            string _ip_port = $"https://{Ip}:{port}";
            IP = _ip_port;
            ip_port = $"{Ip}:{port}";
        }
        else
        {
            string _ip_port = $"http://{Ip}:{port}";
            IP = _ip_port;
            ip_port = $"{Ip}:{port}";
        }
        ServerManager.Start(Ip, port, ssl);

        // DatabaseType
        var databaseType = ConfigINI.Read("Config.ini","Database", "DatabaseType");
        if (!int.TryParse(databaseType, out int db_Type))
        {
            return false;
        }
        switch (db_Type)
        {
            case 0:
                Database = new JsonDatabase();
                break;
            /* //Temp disable.
            case 1:
                Database = new LiteDB_Database();
                break;
            case 2:
                Database = new MySQL_Database();
                break;
            */
            default:
                return false;
        }
        Database.Create();
        ModLoader.LoadMods();

        //start game server(s)
        bool LaunchGameServerInstant = ConfigINI.Read<bool>("Config.ini", "GameServer", "LaunchGameServerInstant");
        string ServerPath = ConfigINI.Read("Config.ini", "GameServer", "ServerPath");
        if (LaunchGameServerInstant && !string.IsNullOrEmpty(ServerPath))
        {
            Process.EnterDebugMode();
            var proc = Process.Start(new ProcessStartInfo()
            {
                FileName = ServerPath,
            });
            if (proc != null)
                GameServerProcesses.Add(proc);
            else
                Console.WriteLine("GameServer was not started!");
        }
        return true;
    }

    /// <summary>
    /// Stopping the Server
    /// </summary>
    public static void Stop()
    {
        if (IsAlreadyQuited)
            return;
        foreach (var item in GameServerProcesses)
        {
            item.Kill();
            item.Close();
            item.Dispose();
        }

        GameServerProcesses.Clear();
        ModLoader.UnloadMods();
        ServerManager.Stop();
        IsAlreadyQuited = true;
    }
}
