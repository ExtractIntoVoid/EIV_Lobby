using LobbyLib.INI;
using ModdableWebServer.Helper;
using ModdableWebServer.Interfaces;
using ModdableWebServer.Servers;
using NetCoreServer;
using Serilog;
using System.Runtime.Intrinsics.X86;

namespace LobbyLib.Web;

/// <summary>
/// Provides a management for web server.
/// </summary>
public static class ServerManager
{
    /// <summary>
    /// Gets the ip and port for the web server.
    /// </summary>
    public static string IpPort_Http { get; private set; } = "http://127.0.0.1:7777/";

    /// <summary>
    /// Gets the ip and port for the web socket server.
    /// </summary>
    public static string IpPort_WS { get; private set; } = "ws://127.0.0.1:7777/";

    /// <summary>
    /// Gets the ip and port the server is running on.
    /// </summary>
    public static string IpPort { get; private set; } = "127.0.0.1:7777";

    /// <summary>
    /// Gets the web socket server.
    /// </summary>
    public static IWSServer? Server { get; private set; }

    /// <summary>
    /// Starts the server.
    /// </summary>
    public static void Start()
    {
        bool sslEnabled = MainIni.Instance.SSL.Enabled;

        string ip = MainIni.Instance.Lobby.Address;
        int port = MainIni.Instance.Lobby.Port;

        if (sslEnabled && string.IsNullOrEmpty(MainIni.Instance.SSL.PfxPath) && string.IsNullOrEmpty(MainIni.Instance.SSL.PfxPasword))
        {
            Log.Information("PfxPath and PfxPassword not declared, SSL is disabled.");
            sslEnabled = false;
        }

        if (sslEnabled)
        {
            SslContext? context;
            if (MainIni.Instance.SSL.VerifyCertificate)
            {
                context = CertHelper.GetContextNoValidate(MainIni.Instance.SSL.Protocol, MainIni.Instance.SSL.PfxPath, MainIni.Instance.SSL.PfxPasword);
            }
            else
            {
                context = CertHelper.GetContext(MainIni.Instance.SSL.Protocol, MainIni.Instance.SSL.PfxPath, MainIni.Instance.SSL.PfxPasword);
            }

            Server = new WSS_Server(context, ip, port)
            {
                DoReturn404IfFail = false,
            };

            ((WSS_Server)Server)!.OverrideAttributes(typeof(LobbyWeb));

            Server.Start();
        }
        else
        {
            Server = new WS_Server(ip, port)
            {
                DoReturn404IfFail = false,
            };

            ((WS_Server)Server)!.OverrideAttributes(typeof(LobbyWeb));

            Server.Start();
        }

        IpPort_Http = sslEnabled ? $"https://{ip}:{port}/" : $"http://{ip}:{port}/";
        IpPort_WS = sslEnabled ? $"wss://{ip}:{port}/" : $"ws://{ip}:{port}/";
        IpPort = $"{ip}:{port}";
        Log.Information("Server started on " + IpPort_Http + " | " + IpPort_WS);
    }

    /// <summary>
    /// Stops the server.
    /// </summary>
    public static void Stop()
    {
        Server?.Stop();
        Server = null;

        Log.Information("Server stopped.");
    }
}
