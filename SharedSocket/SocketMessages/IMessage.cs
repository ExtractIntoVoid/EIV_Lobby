using MemoryPack;

namespace SharedSocket.SocketMessages;

[MemoryPackUnion(0, typeof(KeepAlive))]
[MemoryPackUnion(1, typeof(OnPlayerConnection))]
[MemoryPackUnion(2, typeof(SyncPlayerList))]
[MemoryPackUnion(3, typeof(SyncMaps))]
public partial interface IMessage;