using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SocketUdsClient : UdsClient
{
    public event Received? ReceivedEvent;

    public SocketUdsClient(string path) : base(path) { }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        ReceivedEvent?.Invoke(buffer.Skip((int)offset).Take((int)size).ToArray());
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Unix Domain Socket client caught an error with code {error}");
    }
}