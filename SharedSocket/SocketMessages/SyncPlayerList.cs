using MessagePack;

namespace SharedSocket.SocketMessages;

[MessagePackObject]
public class SyncPlayerList : IMessage
{
    [Key(0)]
    public List<string> UserIds { get; set; } = [];
}
