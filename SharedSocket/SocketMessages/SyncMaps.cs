using MemoryPack;

namespace SharedSocket.SocketMessages;

[MemoryPackable]
public partial class SyncMaps : IMessage
{
    public List<Map> Maps { get; set; } = [];
}

[MemoryPackable]
public partial class Map
{
    public string Name { get; set; } = string.Empty;
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
}
