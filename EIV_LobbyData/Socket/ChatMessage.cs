namespace EIV_LobbyData.Socket;

/// <summary>
/// Represent a "/Socket/Chat" message.
/// </summary>
/// <param name="Sender">The sender id.</param>
/// <param name="Receiver">The receiver id.</param>
/// <param name="Message">The message content.</param>
/// <param name="SentTime">The time when the message sent.</param>
public record class ChatMessage(Guid Sender, Guid Receiver, string Message, DateTimeOffset SentTime)
{
    /// <summary>
    /// The max length for the message.
    /// </summary>
    /// <remarks>
    /// Message dropped if goes beyond.
    /// </remarks>
    public const int MaxMessageLength = 200;
}