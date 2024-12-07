﻿using EIV_Common;
using EIV_Common.Coroutines;
using LobbyLib.Database;
using LobbyLib.Managers;
using LobbyLib.Modding;
using LobbyLib.Web;

namespace LobbyLib;

public class MainControl
{
    static Coroutine? QueueRunner;
    static Coroutine? ProcessRunner;
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
        CoroutineWorkerCustom.KillCoroutines();
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
