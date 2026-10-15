namespace EIV_LobbyData;

/// <summary>
/// Represent a ticket model.
/// </summary>
public class TicketModel
{
    /// <summary>
    /// Gets or sets the user profile id.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Gets or sets the ticket creation time.
    /// </summary>
    public required DateTimeOffset CreatedTime { get; set; }

    /// <summary>
    /// Gets or sets the ticket data.
    /// </summary>
    public string ExtraTicketData { get; set; } = string.Empty;
}
