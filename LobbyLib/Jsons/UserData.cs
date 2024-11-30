namespace LobbyLib.Jsons;

public class UserData
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RSA_PubKey_XML { get; set; } = string.Empty;
    public List<string> FriendsIds { get; set; } = new();
}
