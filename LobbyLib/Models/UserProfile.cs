using EIV_Profile;

namespace LobbyLib.Models;

/// <summary>
/// Represent a saved user character profile.
/// </summary>
public class UserProfile : CoreUserModel
{
    /// <summary>
    /// Gets or sets the character.
    /// </summary>
    public UserCharacter Character { get; set; } = new();
}
