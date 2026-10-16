using EIV_LobbyData;
using EIV_LobbyData.Socket.CustomMessages;
using ModdableWebServer.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// An interface for <see cref="ICustomMessage"/> processor.
/// </summary>
public interface IProcessor
{
    /// <summary>
    /// Process <paramref name="message"/>.
    /// </summary>
    /// <param name="ticket">The ticket.</param>
    /// <param name="session">The session.</param>
    /// <param name="message">The message.</param>
    /// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
    public bool Process([NotNull] TicketModel ticket, [NotNull] IWSSession session, ICustomMessage message);
}
