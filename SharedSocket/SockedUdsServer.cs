using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SockedUdsServer : UdsServer
{
    public SockedUdsServer(string path) : base(path) { }

    protected override UdsSession CreateSession() { return new SocketUdsSession(this); }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Unix Domain Socket server caught an error with code {error}");
    }
}
