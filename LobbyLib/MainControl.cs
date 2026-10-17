using EIV_Coroutines;
using LobbyLib.Database;
using LobbyLib.INI;
using LobbyLib.Managers;
using LobbyLib.Web;
using Serilog;

namespace LobbyLib;

/// <summary>
/// Provides main lobby control.
/// </summary>
public static class MainControl
{
    private static CoroutineHandle? queueRunner;
    private static CoroutineHandle? processRunner;
    private static CoroutineHandle? checkProcesssHandle;
    private static bool isAlreadyQuited;

    /// <summary>
    /// Gets the database.
    /// </summary>
    public static IDatabase Database { get; internal set; } = new EmptyDatabase();

    /// <summary>
    /// Init all managers and starts the server.
    /// </summary>
    /// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
    public static bool InitAll()
    {
        LogManager.Create();

        GameDataStorage.Init();
        PipeDataManager.Init();

        // Init custom coroutine. not doing much but we need later on.
        CoroutineFloatManager.HasAnyCoroutines();

        ServerManager.Start();

        switch (MainIni.Instance.GameData.Database)
        {
            case 0:
                Database = new JsonDatabase();
                break;
            case 1:
                Database = new LiteDB_Database();
                break;
            default:
                return false;
        }

        Database.Create();

        GameStartManager.ControlInit();

        checkProcesssHandle = CoroutineFloatManager.CallPeriodically(TimeSpan.FromSeconds(5), GameStartManager.CheckProcesss);

        // QueueRunner = CoroutineFloatManager.CallPeriodically(TimeSpan.FromSeconds(100), QueueManager.CheckQueue);
        // ProcessRunner = CoroutineFloatManager.CallPeriodically(TimeSpan.FromSeconds(100), GameStartManager.CheckProcesss);
        return true;
    }

    /// <summary>
    /// Stops the server.
    /// </summary>
    public static void Stop()
    {
        if (isAlreadyQuited)
        {
            return;
        }

        if (queueRunner != null)
        {
            CoroutineFloatManager.KillCoroutine(queueRunner.Value);
        }

        queueRunner = null;
        if (processRunner != null)
        {
            CoroutineFloatManager.KillCoroutine(processRunner.Value);
        }

        processRunner = null;

        if (checkProcesssHandle != null)
        {
            CoroutineFloatManager.KillCoroutine(checkProcesssHandle.Value);
        }

        checkProcesssHandle = null;

        ServerManager.Stop();
        isAlreadyQuited = true;

        LogManager.Close();
    }
}
