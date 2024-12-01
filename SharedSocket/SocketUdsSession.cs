using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SocketUdsSession(UdsServer server) : UdsSession(server)
{
    public event ReceivedSession? ReceivedEvent;
    public SocketUdsServer SockedUdsServer => (SocketUdsServer)Server;

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        ReceivedEvent?.Invoke(this, buffer.Skip((int)offset).Take((int)size).ToArray());
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat Unix Domain Socket session caught an error with code {error}");
    }
}
