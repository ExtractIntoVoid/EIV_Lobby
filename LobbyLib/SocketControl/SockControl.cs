﻿using MemoryPack;
using SharedSocket;
using SharedSocket.SocketMessages;

namespace LobbyLib.SocketControl;

public class SockControl
{
    static List<SocketUdsServer> Servers = [];

    public static int PlayerNumber { get; internal set; }

    public static void RunServer(string ip, int port)
    {
        //LobbySocket_{ SHA1(LobbbyIPPort_GameServerPort)}.sock
        var path = $"LobbySocket_{ip}_{port}.sock";
        SocketUdsServer sockedUdsServer = new(path);
        sockedUdsServer.Connected += SockedUdsServer_Connected;
        sockedUdsServer.Disconnected += SockedUdsServer_Disconnected;
        sockedUdsServer.Start();
        Servers.Add(sockedUdsServer);
    }
    public static void StopServer(string ip, int port)
    {
       var server = Servers.FirstOrDefault(x=>x.Path == $"LobbySocket_{ip}_{port}.sock");
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
            Console.WriteLine("Cannot Deser as IMessage, disconnecting!");
            session.Disconnect();
        }
        switch (message)
        {
            case KeepAlive keepAlive:
                {
                    Thread.Sleep(10);
                    session.Send(MemoryPackSerializer.Serialize<IMessage>(keepAlive));
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
            default:
                break;
        }
    }
}