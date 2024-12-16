using MemoryPack;

namespace SharedSocket.SocketMessages;

[MemoryPackable]
public partial class SendMessageToServer<T>
{
    public required string FunctionName { get; set; }
    public required string Message { get; set; }
    public required int MessageId { get; set; }
    public T? Result { get; set; }
}