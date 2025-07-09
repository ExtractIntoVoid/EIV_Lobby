namespace LobbyLib.Models;

public class TicketModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string UserId { get; set; }= string.Empty;
    public ulong Permission { get; set; }
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;
}
