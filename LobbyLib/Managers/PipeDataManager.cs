using EIV_Coroutines;
using EIV_LobbyData.Pipe;
using Serilog;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace LobbyLib.Managers;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class PipeDataManager
{
    public static Collection<string> Maps { get; } = [];

    public static void Init()
    {
        PipeCommunication.OnMessageReceived += PipeCommunication_OnMessageReceived;
    }

    private static void PipeCommunication_OnMessageReceived(Models.PipeServer server, string? data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<IPipeMessage>(out var typeInfo))
        {
            return;
        }

        typeInfo.Options.WriteIndented = false;

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            IPipeMessage? message = JsonSerializer.Deserialize(data, typeInfo);
            if (message == null)
            {
                return;
            }

            Log.Information("Message received! {data}", message);
            switch (message)
            {
                case KeepAlive:
                    {
                        CoroutineFloatManager.CallDelayed(TimeSpan.FromSeconds(10), () =>
                        {
                            server.MessageQueue.Enqueue(data);
                        });
                    }

                    return;
                case MapSync mapSync:
                    {
                        Maps.Clear();
                        foreach (var item in mapSync.Maps.Select(x => x.Name))
                        {
                            Maps.Add(item);
                        }
                    }

                    return;
                default:
                    Log.Warning("Unknown message received! {data}", message);
                    return;
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error on pipe receive communication {ex}", ex);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
