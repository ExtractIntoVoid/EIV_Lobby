using EIV_Common;
using LobbyLib.Database;
using LobbyLib.Modding;
using LobbyLib.Web;
using System.Diagnostics;

namespace LobbyLib;

public class MainControl
{
    public static bool IsAlreadyQuited { get; internal set; } = false;
    public static string IP { get; internal set; } = "https://127.0.0.1:7777";
    public static string Ip_Port { get; internal set; } = "127.0.0.1:7777";
    public static IDatabase Database { get; internal set; } = new EmptyDatabase();

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
            Ip_Port = $"{Ip}:{port}";
        }
        else
        {
            string _ip_port = $"http://{Ip}:{port}";
            IP = _ip_port;
            Ip_Port = $"{Ip}:{port}";
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
        return true;
    }

    /// <summary>
    /// Stopping the Server
    /// </summary>
    public static void Stop()
    {
        if (IsAlreadyQuited)
            return;
        ModLoader.UnloadMods();
        ServerManager.Stop();
        IsAlreadyQuited = true;
    }
}
