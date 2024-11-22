using MessagePack;

namespace SharedSocket.SocketMessages;

[MessagePackObject]
public class KeepAlive : IMessage;