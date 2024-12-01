using MemoryPack;

namespace SharedSocket.SocketMessages;

[MemoryPackable]
public partial class OnPlayerConnection : IMessage
{
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Control if player is connected or disconnected
    /// </summary>
    public bool IsConnected { get; set; }
}
