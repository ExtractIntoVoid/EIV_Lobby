using EIV_LobbyData;
using EIV_LobbyData.Socket;
using EIV_LobbyData.Socket.CustomMessages;
using ModdableWebServer.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Processing basic information..
/// </summary>
/// <typeparam name="TMessage">The message type.</typeparam>
public abstract class BaseProcessor<TMessage> : IProcessor
    where TMessage : ICustomMessage
{
    /// <summary>
    /// Gets the ticket.
    /// </summary>
    protected TicketModel Ticket { get; private set; } = default!;

    /// <summary>
    /// Gets the websocket session.
    /// </summary>
    protected IWSSession Session { get; private set; } = default!;

    /// <summary>
    /// Gets the custom message json type.
    /// </summary>
    protected JsonTypeInfo<ICustomMessage> CustomMessageInfo { get; private set; } = default!;

    /// <inheritdoc/>
    public bool Process([NotNull] TicketModel ticket, [NotNull] IWSSession session, ICustomMessage message)
    {
        Ticket = ticket;
        Session = session;

        if (!JsonSerializerConvert.TryGetTypeInfo<ICustomMessage>(out var typeInfo))
        {
            return false;
        }

        CustomMessageInfo = typeInfo;

        if (message is not TMessage tmessage)
        {
            return false;
        }

        return OnProcess(tmessage);
    }

    /// <summary>
    /// Sends a client response.
    /// </summary>
    /// <param name="code">The client response code.</param>
    /// <returns>Always <see langword="true"/>.</returns>
    public bool SendResponse(ResultCode code)
    {
        if (!JsonSerializerConvert.TryGetTypeInfo<ClientResponse>(out var clientResponseTypeInfo))
        {
            return false;
        }

        ClientResponse response = new(code == ResultCode.Success, (uint)ResultCode.Success, code.GetMessage());
        string data = JsonSerializer.Serialize(response, clientResponseTypeInfo);
        Session.SendText(data);
        return true;
    }

    /// <summary>
    /// Sends a client response.
    /// </summary>
    /// <param name="message">The client message to send.</param>
    /// <returns>Always <see langword="true"/>.</returns>
    public bool SendResponse(ICustomMessage message)
    {
        string data = JsonSerializer.Serialize(message, CustomMessageInfo);
        Session.SendText(data);
        return true;
    }

    /// <summary>
    /// Processing <paramref name="message"/>.
    /// </summary>
    /// <param name="message">Message to process.</param>
    /// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
    protected abstract bool OnProcess([NotNull] TMessage message);
}
