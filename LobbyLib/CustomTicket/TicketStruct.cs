namespace LobbyLib.CustomTicket;

public struct TicketStruct
{
    public Guid Id;
    public string Name;
    public string UserId;
    public ulong Permission;
    public DateTimeOffset CreatedTime;
}
