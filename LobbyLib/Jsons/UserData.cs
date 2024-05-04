namespace LobbyLib.Jsons
{
    public class UserData
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RSA_PubKey_XML { get; set; }
        public List<string> FriendsIds { get; set; } = new();
    }
}
