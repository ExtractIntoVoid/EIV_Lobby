using MessagePack;

namespace SharedSocket.SocketMessages;

[MessagePackObject]
public class OnPlayerConnection : IMessage
{
    [Key(0)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Control if player is connected or disconnected
    /// </summary>
    [Key(1)]
    public bool IsConnected { get; set; }
}
