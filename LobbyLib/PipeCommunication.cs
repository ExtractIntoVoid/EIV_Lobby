using EIV_Coroutines;
using System.Collections.Concurrent;
using System.IO.Pipes;

namespace LobbyLib;

/// <summary>
/// Provides a communication via named pipe.
/// </summary>
public static class PipeCommunication
{
    private static readonly Dictionary<string, NamedPipeServerStream> Servers = [];

    private static readonly ConcurrentDictionary<string, ConcurrentQueue<byte[]>> SenderQueue = new();

    /// <summary>
    /// Called when any data received.
    /// </summary>
#pragma warning disable CA1003 // Use generic event handler instances
    public static event Action<byte[]>? OnReceived;
#pragma warning restore CA1003 // Use generic event handler instances

    /// <summary>
    /// Gets the communication ids.
    /// </summary>
    public static IReadOnlyCollection<string> CommIds => Servers.Keys;

    /// <summary>
    /// Starts the communication via <paramref name="commId"/>.
    /// </summary>
    /// <param name="commId">The server commId.</param>
    public static void StartServer(string commId)
    {
        NamedPipeServerStream server = new(commId, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
        CoroutineFloatManager.StartCoroutine(Read(server), $"{nameof(PipeCommunication)}_{commId}");
        CoroutineFloatManager.StartCoroutine(Write(server, commId), $"{nameof(PipeCommunication)}_{commId}");
        Servers[commId] = server;
    }

    /// <summary>
    /// Stops the communcation.
    /// </summary>
    /// <param name="commId">The server commId.</param>
    public static void StopServer(string commId)
    {
        if (!Servers.Remove(commId, out NamedPipeServerStream? server))
        {
            return;
        }

        server.Disconnect();
        server.Dispose();
        CoroutineFloatManager.KillCoroutineTag<float>($"{nameof(PipeCommunication)}_{commId}");
    }

    /// <summary>
    /// Sends the data to the named pipe.
    /// </summary>
    /// <param name="commId">The commId to send to.</param>
    /// <param name="data">The data.</param>
    public static void Send(string commId, ReadOnlySpan<byte> data)
    {
        Send(commId, data.ToArray());
    }

    /// <summary>
    /// Sends the data to the named pipe.
    /// </summary>
    /// <param name="commId">The commId to send to.</param>
    /// <param name="data">The data.</param>
    public static void Send(string commId, byte[] data)
    {
        if (!Servers.ContainsKey(commId))
        {
            return;
        }

        SenderQueue.AddOrUpdate(
            commId,
            (p) =>
            {
                ConcurrentQueue<byte[]> q = SenderQueue.GetOrAdd(commId, new ConcurrentQueue<byte[]>());
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

    private static IEnumerator<float> Write(NamedPipeServerStream server, string commId)
    {
        if (server == null)
        {
            yield break;
        }

        while (true)
        {
            if (server.IsConnected && server.CanWrite && SenderQueue.TryGetValue(commId, out var queue) && queue.TryDequeue(out byte[]? queueBytes) && queueBytes != null)
            {
                server.Write(queueBytes);
                server.Flush();
            }

            yield return 0;
        }
    }
}
