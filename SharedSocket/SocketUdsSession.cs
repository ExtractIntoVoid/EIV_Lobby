using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SocketUdsSession : UdsSession
{
    public SocketUdsSession(UdsServer server) : base(server) { }

    public event Received? ReceivedEvent;
    public SockedUdsServer SockedUdsServer => (SockedUdsServer)Server;

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        ReceivedEvent?.Invoke(buffer.Skip((int)offset).Take((int)size).ToArray());
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat Unix Domain Socket session caught an error with code {error}");
    }
}
