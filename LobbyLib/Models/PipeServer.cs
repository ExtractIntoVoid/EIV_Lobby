using System.Collections.Concurrent;
using System.IO.Pipes;

namespace LobbyLib.Models;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record class PipeServer(
    string Name,
    NamedPipeServerStream Stream,
    StreamReader Reader,
    StreamWriter Writer,
    ConcurrentQueue<string> MessageQueue);
