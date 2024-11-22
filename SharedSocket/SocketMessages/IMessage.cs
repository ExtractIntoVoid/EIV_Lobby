using MessagePack;

namespace SharedSocket.SocketMessages;

[Union(0, typeof(KeepAlive))]
[Union(1, typeof(OnPlayerConnection))]
public interface IMessage;