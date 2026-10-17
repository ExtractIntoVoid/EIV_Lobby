using EIV_Coroutines;
using LobbyLib.Models;
using Serilog;
using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Text;

namespace LobbyLib;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Provides a communication via named pipe.
/// </summary>
public static class PipeCommunication
{
    private static readonly ConcurrentDictionary<string, PipeServer> Servers = [];

    /// <summary>
    /// Called when any data received.
    /// </summary>
#pragma warning disable CA1003 // Use generic event handler instances
    public static event Action<PipeServer, string?>? OnMessageReceived;

    public static event Action? OnClientConnected;

    public static event Action? OnClientDisconnected;
#pragma warning restore CA1003 // Use generic event handler instances

    /// <summary>
    /// Gets the communication ids.
    /// </summary>
    public static IEnumerable<string> CommIds => Servers.Keys;

    /// <summary>
    /// Starts the communication via <paramref name="commId"/>.
    /// </summary>
    /// <param name="commId">The server commId.</param>
    public static void StartServer(string commId)
    {
        NamedPipeServerStream server = new(commId, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
#pragma warning disable CA2000 // Dispose objects before losing scope
        PipeServer pipeServer = new(
            commId,
            server,
            new StreamReader(server, Encoding.UTF8),
            new StreamWriter(server, Encoding.UTF8)
            {
                NewLine = "\n",
                AutoFlush = false,
            },
            []);

        Servers.TryAdd(commId, pipeServer);
#pragma warning restore CA2000 // Dispose objects before losing scope

        Thread waitForConnect = new(WaitForConnectThread)
        {
            IsBackground = true,
            Name = $"WaitForConnectThread_{commId}",
        };
        waitForConnect.Start(commId);

        Thread sendThread = new(SendMessageThread)
        {
            IsBackground = true,
            Name = $"SendMessageThread_{commId}",
        };
        sendThread.Start(commId);

        Thread readThread = new(ReadMessageThread)
        {
            IsBackground = true,
            Name = $"ReadMessageThread_{commId}",
        };
        readThread.Start(commId);
    }

    /// <summary>
    /// Stops the communcation.
    /// </summary>
    /// <param name="commId">The server commId.</param>
    public static void StopServer(string commId)
    {
        if (!Servers.TryRemove(commId, out PipeServer? server))
        {
            return;
        }

        server.Stream.Disconnect();
        server.Stream.Dispose();

        server.MessageQueue.Clear();
    }

    private static void WaitForConnectThread(object? obj)
    {
        if (obj is not string serverId)
        {
            return;
        }

        try
        {
            if (Servers.TryGetValue(serverId, out var server) &&
                server.Stream != null &&
                !server.Stream.IsConnected)
            {
                server.Stream.WaitForConnection();
            }
        }
        catch (Exception ex) when (ex is not IOException)
        {
            Log.Error("WaitForConnectThread error! {ex}", ex);
        }

        Log.Verbose("Pipe {id} connected from unity!", serverId);
    }

    private static void SendMessageThread(object? obj)
    {
        if (obj is not string serverId)
        {
            return;
        }

        while (Servers.TryGetValue(serverId, out var server) &&
            server.Stream != null &&
            server.Writer != null)
        {
            if (!server.Stream.IsConnected)
            {
                continue;
            }

            if (!server.MessageQueue.TryDequeue(out string? msg))
            {
                Thread.Sleep(5);
                continue;
            }

            try
            {
                server.Writer.WriteLine(msg);
                server.Writer.Flush();
            }
            catch (Exception ex) when (ex is not ObjectDisposedException)
            {
                Log.Error("Pipe write thrown error! {ex}", ex);
            }
        }

        Log.Verbose("Pipe {id} sending finished!", serverId);
    }

    private static void ReadMessageThread(object? obj)
    {
        if (obj is not string serverId)
        {
            return;
        }

        while (Servers.TryGetValue(serverId, out var server) &&
            server.Stream != null &&
            server.Reader != null)
        {
            try
            {
                if (!server.Stream.IsConnected)
                {
                    continue;
                }

                string? line = server.Reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                CoroutineFloatManager.CallDelayed(
                         0.1f,
                         () =>
                         {
                             OnMessageReceived?.Invoke(server, line);
                         },
                         nameof(PipeCommunication));
            }
            catch (Exception ex) when (ex is not ObjectDisposedException)
            {
                Log.Error("Pipe write thrown error! {ex}", ex);
            }
        }

        Log.Verbose("Pipe {id} reading finished!", serverId);
    }
}
