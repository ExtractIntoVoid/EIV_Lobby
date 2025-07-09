using EIV_Common.Coroutines;
using LobbyLib.Managers;
using MemoryPack;
using SharedSocket;
using SharedSocket.SocketMessages;

namespace LobbyLib.SocketControl;

public class SockControl
{
    static List<SocketUdsServer> Servers = [];

    public static int PlayerNumber { get; internal set; }

    public static event Action<SocketUdsSession, IMessage>? OnMessageReceived;

    public static void StartServer(int port)
    {
        var path = $"LobbySocket_{port}.sock";
        SocketUdsServer sockedUdsServer = new(path);
        sockedUdsServer.Connected += SockedUdsServer_Connected;
        sockedUdsServer.Disconnected += SockedUdsServer_Disconnected;
        sockedUdsServer.Start();
        Servers.Add(sockedUdsServer);
    }

    public static void StopServer(int port)
    {
       var server = Servers.FirstOrDefault(x => x.Path == $"LobbySocket_{port}.sock");
        if (server == null)
            return;
        server.Connected += SockedUdsServer_Connected;
        server.Disconnected += SockedUdsServer_Disconnected;
        server.Stop();
    }

    private static void SockedUdsServer_Disconnected(SocketUdsSession session, bool IsDisconnected)
    {
        session.ReceivedEvent -= Session_ReceivedEvent;
        session.SockedUdsServer.Connected -= SockedUdsServer_Connected;
        session.SockedUdsServer.Disconnected -= SockedUdsServer_Disconnected;
    }

    private static void SockedUdsServer_Connected(SocketUdsSession session, bool IsDisconnected)
    {
        session.ReceivedEvent += Session_ReceivedEvent;
    }

    private static void Session_ReceivedEvent(SocketUdsSession session, ReadOnlySpan<byte> data)
    {
        IMessage? message = MemoryPackSerializer.Deserialize<IMessage>(data);
        if (message == null)
        {
            Console.WriteLine("Cannot Deserialize as IMessage, disconnecting!");
            session.Disconnect();
        }
        switch (message)
        {
            case KeepAlive keepAlive:
                {
                    CoroutineWorkerCustom.CallDelayed(TimeSpan.FromMilliseconds(10), () => 
                    {
                        session.Send(MemoryPackSerializer.Serialize<IMessage>(keepAlive));
                    });
                }
                break;
            case OnPlayerConnection onPlayerConnection:
                {
                    if (onPlayerConnection.IsConnected)
                        Console.WriteLine($"New player connected! {onPlayerConnection.UserId}");
                    else
                        Console.WriteLine($"Player disconnected! {onPlayerConnection.UserId}");
                }
                break;
            case SyncPlayerList syncPlayerList:
                {
                    PlayerNumber = syncPlayerList.UserIds.Count;
                }
                break;
            case SyncMaps syncMaps:
                {
                    foreach (var map in syncMaps.Maps)
                    {
                        QueueManager.Maps.Add(new()
                        { 
                            Name = map.Name,
                            MinPlayer = map.MinPlayers,
                            MaxPlayer = map.MaxPlayers,
                        });
                    }
                }
                break;
            default:
                OnMessageReceived?.Invoke(session, message);
                break;
        }
    }
}
