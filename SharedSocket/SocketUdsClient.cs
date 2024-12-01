using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SocketUdsClient(string path) : UdsClient(path)
{
    public event ReceivedClient? ReceivedEvent;

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        ReceivedEvent?.Invoke(this, buffer.Skip((int)offset).Take((int)size).ToArray());
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Unix Domain Socket client caught an error with code {error}");
    }
}