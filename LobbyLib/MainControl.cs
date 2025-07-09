using EIV_Common;
using EIV_Common.Coroutines;
using LobbyLib.Database;
using LobbyLib.Managers;
using LobbyLib.Modding;
using LobbyLib.Web;

namespace LobbyLib;

public class MainControl
{
    static CoroutineHandle? QueueRunner;
    static CoroutineHandle? ProcessRunner;
    public static bool IsAlreadyQuited { get; internal set; } = false;
    public static string IP { get; internal set; } = "https://127.0.0.1:7777";
    public static string Ip_Port { get; internal set; } = "127.0.0.1:7777";
    public static IDatabase Database { get; internal set; } = new EmptyDatabase();

    /// <summary>
    /// Init the Server
    /// </summary>
    public static bool InitAll()
    {
        // Init custom coroutine. not doing much but we need later on.
        CoroutineWorkerCustom.HasAnyCoroutines();
        // ini check
        if (!File.Exists("Config.ini"))
        {
            File.WriteAllText("Config.ini", LobbyLib_Res.Config);
        }

        bool ssl = ConfigINI.Read<bool>("Config.ini", "Lobby", "EnableSSL");
        string Ip = ConfigINI.Read("Config.ini", "Lobby", "ServerAddress");
        ushort port = ConfigINI.Read<ushort>("Config.ini", "Lobby", "ServerPort");

        if (ssl && string.IsNullOrEmpty(ConfigINI.Read("config.ini", "Lobby", "PfxPath")) && string.IsNullOrEmpty(ConfigINI.Read("config.ini", "Lobby", "PfxPasword")))
        {
            Console.WriteLine("PfxPath and PfxPassword not declared, SSL is disabled.");
            ssl = false;
        }
        IP = $"http{(ssl ? "s" : string.Empty)}://{Ip}:{port}";
        Ip_Port = $"{Ip}:{port}";
        ServerManager.Start(Ip, port, ssl);

        // DatabaseType
        var databaseType = ConfigINI.Read("Config.ini","Database", "DatabaseType");
        if (!int.TryParse(databaseType, out int db_Type))
            return false;
        switch (db_Type)
        {
            case 0:
                Database = new JsonDatabase();
                break;
            /* //Temp disable.
            case 1:
                Database = new LiteDB_Database();
                break;
            */
            default:
                return false;
        }
        Database.Create();
        ModLoader.LoadMods();
        GameStartManager.ControlInit();
        QueueRunner = CoroutineWorkerCustom.CallPeriodically(TimeSpan.FromSeconds(100), QueueManager.CheckQueue);
        ProcessRunner = CoroutineWorkerCustom.CallPeriodically(TimeSpan.FromSeconds(100), GameStartManager.CheckProcesss);
        return true;
    }

    /// <summary>
    /// Stopping the Server
    /// </summary>
    public static void Stop()
    {
        if (IsAlreadyQuited)
            return; 
        if (QueueRunner != null)
            CoroutineWorkerCustom.KillCoroutineInstance(QueueRunner.Value);
        QueueRunner = null;
        if (ProcessRunner != null)
            CoroutineWorkerCustom.KillCoroutineInstance(ProcessRunner.Value);
        ProcessRunner = null;
        ModLoader.UnloadMods();
        ServerManager.Stop();
        IsAlreadyQuited = true;
    }
}
