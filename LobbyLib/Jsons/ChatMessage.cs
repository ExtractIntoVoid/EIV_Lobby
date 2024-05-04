namespace LobbyLib.Jsons
{
    public class ChatMessage
    {
        public const int MAX_MESSAGE_LEN = 200;

        /// <summary>
        /// Message Id
        /// </summary>
        public required ulong Id;
        /// <summary>
        /// UserId of the Sender
        /// </summary>
        public required string SenderId;

        /// <summary>
        /// UserId or a PartyId of a Receiver
        /// </summary>
        public required string ReceiverId;

        /// <summary>
        /// Message sent with data. Sanitize!
        /// </summary>
        public required string Message;

        /// <summary>
        /// Time when the message sent.
        /// </summary>
        public required DateTimeOffset SendTime;
    }
}
