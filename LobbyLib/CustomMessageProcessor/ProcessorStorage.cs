using EIV_LobbyData;
using EIV_LobbyData.Socket.CustomMessages;
using ModdableWebServer.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Storage for custom message processor.
/// </summary>
public static class ProcessorStorage
{
    /// <summary>
    /// Gets the custom processor.
    /// </summary>
    public static Collection<IProcessor> Processors { get; } =
    [
        new FriendActionProcessor(),
    ];

    /// <summary>
    /// Run a processor.
    /// </summary>
    /// <param name="ticket">The ticket.</param>
    /// <param name="session">The session.</param>
    /// <param name="message">The message.</param>
    public static void Process([NotNull] TicketModel ticket, [NotNull] IWSSession session, ICustomMessage message)
    {
        foreach (var item in Processors)
        {
            if (item.Process(ticket, session, message))
            {
                break;
            }
        }
    }
}
