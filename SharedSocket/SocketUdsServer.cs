using NetCoreServer;
using System.Net.Sockets;

namespace SharedSocket;

public class SocketUdsServer(string path) : UdsServer(path)
{
    public List<Guid> ConnectedIds = [];
    public Dictionary<Guid, SocketUdsSession> ConnectedSocketSessions = [];
    public event SessionAction? Disconnected;
    public event SessionAction? Connected;
    public string Path { get; internal set; } = path;

    protected override UdsSession CreateSession()
    { 
        return new SocketUdsSession(this); 
    }

    protected override void OnDisconnected(UdsSession session)
    {
        ConnectedIds.Remove(session.Id);
        ConnectedSocketSessions.Remove(session.Id);
        Disconnected?.Invoke((SocketUdsSession)session, true);
    }

    protected override void OnConnected(UdsSession session)
    {
        ConnectedIds.Add(session.Id);
        ConnectedSocketSessions.Add(session.Id, (SocketUdsSession)session);
        Connected?.Invoke((SocketUdsSession)session, false);
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Unix Domain Socket server caught an error with code {error}");
    }
}
