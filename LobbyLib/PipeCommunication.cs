using EIV_Coroutines;
using System.Collections.Concurrent;
using System.IO.Pipes;

namespace LobbyLib;

/// <summary>
/// Provides a communication via named pipe.
/// </summary>
public static class PipeCommunication
{
    private static readonly Dictionary<int, NamedPipeServerStream> Servers = [];

    private static readonly ConcurrentDictionary<int, ConcurrentQueue<byte[]>> SenderQueue = new();

    /// <summary>
    /// Called when any data received.
    /// </summary>
#pragma warning disable CA1003 // Use generic event handler instances
    public static event Action<byte[]>? OnReceived;
#pragma warning restore CA1003 // Use generic event handler instances

    /// <summary>
    /// Starts the communication via <paramref name="port"/>.
    /// </summary>
    /// <param name="port">The server port.</param>
    public static void StartServer(int port)
    {
        string path = $"Lobby_{port}";
        NamedPipeServerStream server = new(path, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
        CoroutineFloatManager.StartCoroutine(Read(server), $"{nameof(PipeCommunication)}_{port}");
        CoroutineFloatManager.StartCoroutine(Write(server, port), $"{nameof(PipeCommunication)}_{port}");
        Servers[port] = server;
    }

    /// <summary>
    /// Stops the communcation.
    /// </summary>
    /// <param name="port">The server port.</param>
    public static void StopServer(int port)
    {
        if (!Servers.Remove(port, out NamedPipeServerStream? server))
        {
            return;
        }

        server.Disconnect();
        server.Dispose();
        CoroutineFloatManager.KillCoroutineTag<float>($"{nameof(PipeCommunication)}_{port}");
    }

    /// <summary>
    /// Sends the data to the named pipe.
    /// </summary>
    /// <param name="port">The port to send to.</param>
    /// <param name="data">The data.</param>
    public static void Send(int port, ReadOnlySpan<byte> data)
    {
        Send(port, data.ToArray());
    }

    /// <summary>
    /// Sends the data to the named pipe.
    /// </summary>
    /// <param name="port">The port to send to.</param>
    /// <param name="data">The data.</param>
    public static void Send(int port, byte[] data)
    {
        if (!Servers.ContainsKey(port))
        {
            return;
        }

        SenderQueue.AddOrUpdate(
            port,
            (p) =>
            {
                ConcurrentQueue<byte[]> q = SenderQueue.GetOrAdd(port, new ConcurrentQueue<byte[]>());
                q.Enqueue(data);
                return q;
            },
            (p, q) =>
            {
                q.Enqueue(data);
                return q;
            });
    }

    private static IEnumerator<float> Read(NamedPipeServerStream server)
    {
        if (server == null)
        {
            yield break;
        }

        while (true)
        {
            if (server.IsConnected && server.CanRead)
            {
                Span<byte> bytes = new(new byte[server.InBufferSize]);
                int read = server.Read(bytes);
                OnReceived?.Invoke(bytes[..read].ToArray());
            }

            yield return 0;
        }
    }

    private static IEnumerator<float> Write(NamedPipeServerStream server, int port)
    {
        if (server == null)
        {
            yield break;
        }

        while (true)
        {
            if (server.IsConnected && server.CanWrite && SenderQueue.TryGetValue(port, out var queue) && queue.TryDequeue(out byte[]? queueBytes) && queueBytes != null)
            {
                server.Write(queueBytes);
                server.Flush();
            }

            yield return 0;
        }
    }
}
