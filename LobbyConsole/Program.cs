using LobbyLib;
using Serilog;
using System.Runtime.InteropServices;

namespace LobbyConsole;

/// <summary>
/// Main entry point.
/// </summary>
internal sealed class Program
{
    private static readonly string[] QuitCommands = ["q", "quit", "exit"];

    /// <summary>
    /// Main Entry point for lobby console.
    /// </summary>
    internal static void Main()
    {
        if (!MainControl.InitAll())
        {
            Log.Information("Creating Lobby Server failed!");
            return;
        }

        Log.Information("Type 'quit', 'q' or 'exit' to quit shutdown the server.");
        WaitForShutdown();
        MainControl.Stop();
    }

    private static void WaitForShutdown()
    {
        using var stop = new ManualResetEventSlim();
        void Stop(PosixSignalContext context)
        {
            context.Cancel = true;
            stop.Set();
        }

        using PosixSignalRegistration terminate = PosixSignalRegistration.Create(PosixSignal.SIGTERM, Stop);
        using PosixSignalRegistration interrupt = PosixSignalRegistration.Create(PosixSignal.SIGINT, Stop);

        var console = new Thread(() => ReadConsole(stop)) { IsBackground = true, Name = "console" };
        console.Start();
        stop.Wait();
    }

    private static void ReadConsole(ManualResetEventSlim stop)
    {
        while (Console.ReadLine() is { } line)
        {
            if (QuitCommands.Contains(line))
            {
                stop.Set();
                return;
            }

            /*
            if (line.StartsWith('!'))
                CommandController.Run(line[1..]);
            */
        }
    }
}