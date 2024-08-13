using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace LobbyLib.Web;

public class ServerUds
{
    public class LobbyUdsSession : UdsSession
    {
        public LobbyUdsSession(UdsServer server) : base(server) { }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            buffer = buffer.Skip((int)offset).Take((int)size).ToArray();

        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat Unix Domain Socket session caught an error with code {error}");
        }
    }

    public class LobbyUdsServer : UdsServer
    {
        public LobbyUdsServer(string path) : base(path) { }

        protected override UdsSession CreateSession() { return new LobbyUdsSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Unix Domain Socket server caught an error with code {error}");
        }
    }


    public class LobbyUdsClient : UdsClient
    {
        public LobbyUdsClient(string path) : base(path) { }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Console.WriteLine(Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Unix Domain Socket client caught an error with code {error}");
        }
    }
}
