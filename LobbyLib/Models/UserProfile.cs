using EIV_JsonLib.Profile;

namespace LobbyLib.Models;

public class UserProfile
{
    public Guid UserId { get; set; } //ID From UserDB Id
    public UserCharacter Character { get; set; } = new();
}
