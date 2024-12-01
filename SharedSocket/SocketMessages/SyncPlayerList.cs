using MemoryPack;

namespace SharedSocket.SocketMessages;

[MemoryPackable]
public partial class SyncPlayerList : IMessage
{
    public List<string> UserIds { get; set; } = [];
}
