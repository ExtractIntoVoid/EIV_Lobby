using EIV_JsonLib.Lobby;
using EIV_JsonLib.Lobby.Friend;
using LobbyLib.CustomTicket;
using LobbyLib.Web;
using ModdableWebServer;
using NetCoreServer;
using System.Text.Json;

namespace LobbyLib.Managers;

public class FriendManager
{
    public static bool Manage(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketEnum clientSocketEnum, ClientSocketMessage clientSocketMessage)
    {
        switch (clientSocketEnum)
        {
            case ClientSocketEnum.FriendSearch:
                SearchFriend(socketStruct, clientSocketMessage);
                return true;
            // rename to FriendAdd! | this add to Friend Request!
            case ClientSocketEnum.FriendAction:
                AddOrRemoveFriendRequest(socketStruct, ticket, clientSocketMessage);
                return true;
            // todo add FriendRequest. For deny or accept.
            default:
                return false;
        }
    }

    public static void SearchFriend(WebSocketStruct socketStruct, ClientSocketMessage clientSocketMessage)
    {
        FriendSearch? friendSearch = JsonSerializer.Deserialize<FriendSearch>(clientSocketMessage.JsonMessage);
        if (friendSearch == null)
            return;

        if (string.IsNullOrEmpty(friendSearch.SearchParam))
            return;

        // search for friend usually start with what you searching.
        // ie: i: GUID | u: UserId . If there is none probably search for name.

        // Id Search
        if (friendSearch.SearchParam.StartsWith("i: "))
        {
            var users = MainControl.Database.GetUserDatas().Where(x => x.Id.ToString() == friendSearch.SearchParam.RemoveSuffix("i: ")).ToList();

            EIV_Lobby.SendResponse(socketStruct, new FriendSearchResponse()
            {
                UserIds = users.Select(x => x.UserId).ToList(),
            }, ClientSocketEnum.FriendSearchResponse);
        }
        // UserId Search
        else if (friendSearch.SearchParam.StartsWith("u: "))
        {
            var users = MainControl.Database.GetUserDatas().Where(x => x.UserId.Contains(friendSearch.SearchParam.RemoveSuffix("u: "))).ToList();

            EIV_Lobby.SendResponse(socketStruct, new FriendSearchResponse()
            {
                UserIds = users.Select(x => x.UserId).ToList(),
            }, ClientSocketEnum.FriendSearchResponse);
        }
        // fallback to normal name search
        else
        {
            var users = MainControl.Database.GetUserDatas().Where(x => x.Name.Contains(friendSearch.SearchParam)).ToList();

            EIV_Lobby.SendResponse(socketStruct, new FriendSearchResponse()
            { 
                UserIds = users.Select(x => x.UserId).ToList(),
            }, ClientSocketEnum.FriendSearchResponse);
        }
    }

    public static void AddOrRemoveFriendRequest(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        FriendAction? friendAction = JsonSerializer.Deserialize<FriendAction>(clientSocketMessage.JsonMessage);
        if (friendAction == null)
            return;
        var user = MainControl.Database.GetUserData(ticket.Id);
        // early checks for letting our database rest until needed.
        if (user == null)
            return;
        // WTF?
        if (user.BlockList.FriendInviteBlocks.Contains(friendAction.UserId))
            return;
        if (user.FriendRequests.Contains(friendAction.UserId) && !friendAction.ToRemove)
            return;
        if (!user.FriendRequests.Contains(friendAction.UserId) && friendAction.ToRemove)
            return;
        var friend_user = MainControl.Database.GetUserDatas().FirstOrDefault(x => x.UserId == friendAction.UserId);
        // Also WTF?
        if (friend_user == null) 
            return;
        if (friend_user.FriendRequests.Contains(user.UserId) && !friendAction.ToRemove)
            return;
        if (!friend_user.FriendRequests.Contains(user.UserId) && friendAction.ToRemove)
            return;
        if (friend_user.BlockList.FriendInviteBlocks.Contains(ticket.UserId))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = false,
                ErrorCode = 4,
                Message = "You cannot add this user!",
            });
        }
        ClientSocketResponse socketResponse = new()
        {
            IsSuccess = true,
            ErrorCode = 0,
            Message = "You added or removed the request!",
        };
        if (friendAction.ToRemove)
        {
            user.FriendRequests.Remove(friendAction.UserId);
            friend_user.FriendRequests.Remove(user.UserId);
        }
        else
        {
            user.FriendRequests.Add(friendAction.UserId);
            friend_user.FriendRequests.Add(user.UserId);
        }
        EIV_Lobby.SendResponse(socketStruct, socketResponse);
        if (EIV_Lobby.ClientUserToWS.TryGetValue(friendAction.UserId, out var webSocketStruct))
        {
            EIV_Lobby.SendResponse(webSocketStruct, socketResponse);
        }
    }
}
