using EIV_JsonLib.Lobby;
using EIV_JsonLib.Lobby.Group;
using LobbyLib.CustomTicket;
using LobbyLib.Web;
using ModdableWebServer;
using ModdableWebServer.Helper;
using System.Text.Json;

namespace LobbyLib.Managers;
public struct Group
{
    public int Id;
    public string Owner;
    public List<string> UserIds;
    public List<string> InvitedUsers;
    public bool OwnerInvite;
}

public class GroupManager
{
    public static List<Group> Groups { get; set; } = [];

    static int LastSavedGroupId = 1;

    public static bool Manage(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketEnum clientSocketEnum, ClientSocketMessage clientSocketMessage)
    {
        switch (clientSocketEnum)
        {
            case ClientSocketEnum.GroupCreate:
                CreateGroup(socketStruct, ticket);
                return true;
            case ClientSocketEnum.GroupDelete:
                DeleteGroup(socketStruct, ticket);
                return true;
            case ClientSocketEnum.GroupUpdate:
                UpdateGroup(socketStruct, ticket, clientSocketMessage);
                return true;
            case ClientSocketEnum.GroupInvite:
                InviteGroup(data, socketStruct, ticket, clientSocketMessage);
                return true;
            case ClientSocketEnum.GroupInviteResponse:
                InviteGroupResponse(data, socketStruct, ticket, clientSocketMessage);
                return true;
            case ClientSocketEnum.GroupKick:
                KickGroup(data, ticket, clientSocketMessage);
                return true;
            default:
                return false;
        }
    }

    static void CreateGroup(WebSocketStruct socketStruct, TicketStruct ticket)
    {
        if (Groups.Any(x => x.UserIds.Contains(ticket.UserId)))
        {
            ClientSocketResponse response = new()
            {
                IsSuccess = false,
                ErrorCode = 1,
                Message = "You already in a group!",
            };
            EIV_Lobby.SendResponse(socketStruct, response);
            return;
        }
        Group group;
        Groups.Add(group = new Group()
        {
            Id = LastSavedGroupId,
            InvitedUsers = [],
            Owner = ticket.UserId,
            OwnerInvite = true,
            UserIds =
            [
                ticket.UserId,
            ]
        });
        LastSavedGroupId++;
        GroupUpdate groupUpdate = new()
        {
            GroupId = group.Id,
            EnableUserInvites = true,
            Owner = ticket.UserId,
            Players = group.UserIds,
        };
        EIV_Lobby.SendResponse(socketStruct, groupUpdate, ClientSocketEnum.GroupUpdate);
    }

    static void DeleteGroup(WebSocketStruct socketStruct, TicketStruct ticket)
    {
        if (!Groups.Any(x => x.Owner == ticket.UserId))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = false,
                ErrorCode = 2,
                Message = "You are not the Group owner!",
            });
            return;
        }
        var group = Groups.First(x => x.Owner == ticket.UserId);
        // send update the group deleted!

        Groups.Remove(group);
        ClientSocketResponse response = new()
        {
            IsSuccess = true,
            ErrorCode = 0,
            Message = "Deleting group success!",
        };
        EIV_Lobby.SendResponse(socketStruct, response);
    }

    static void UpdateGroup(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        GroupUpdate? groupUpdate = JsonSerializer.Deserialize<GroupUpdate>(clientSocketMessage.JsonMessage);
        if (groupUpdate == null)
            return;
        if (!Groups.Any(x => x.Id != groupUpdate.GroupId))
        {
            // send back we cant. (Hijack?)
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = false,
                ErrorCode = 2,
                Message = "You are not the Group owner!",
            });
            return;
        }
        // todo figure it out
    }

    static void InviteGroup(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        GroupInvite? groupInvite = JsonSerializer.Deserialize<GroupInvite>(clientSocketMessage.JsonMessage);
        if (groupInvite == null)
            return;
        // check if you can send invite to this user.
        var user = MainControl.Database.GetUserData(ticket.Id);
        if (user != null && user.BlockList.GroupInviteBlocks.Contains(groupInvite.Invitee))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = false,
                ErrorCode = 3,
                Message = "You cannot send Invite to this user!",
            });
            return;
        }

        if (!EIV_Lobby.ClientUserToWS.TryGetValue(groupInvite.Invitee, out var webSocketStruct))
        {
            // User not connected to websocket. Throw it out.
            return;
        }
        webSocketStruct.SendWebSocketByteArray(data.ToArray());
    }

    static void InviteGroupResponse(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        GroupInviteResponse? groupInviteResponse = JsonSerializer.Deserialize<GroupInviteResponse>(clientSocketMessage.JsonMessage);
        if (groupInviteResponse == null)
            return;
        var group = Groups.First(x => x.Id == groupInviteResponse.GroupId);
        group.InvitedUsers.Remove(ticket.UserId);
        if (!groupInviteResponse.IsDenied)
            group.UserIds.Add(ticket.UserId);
        if (!EIV_Lobby.ClientUserToWS.TryGetValue(groupInviteResponse.Inviter, out var webSocketStruct))
        {
            // User not connected to websocket. Throw it out.
            return;
        }
        webSocketStruct.SendWebSocketByteArray(data.ToArray());
        EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
        {
            IsSuccess = true,
            ErrorCode = 0,
            Message = "You are not the Group owner!",
        });
    }

    static void KickGroup(ReadOnlySpan<byte> data, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        GroupKick? groupKick = JsonSerializer.Deserialize<GroupKick>(clientSocketMessage.JsonMessage);
        if (groupKick == null)
            return;
        if (!EIV_Lobby.ClientUserToWS.TryGetValue(groupKick.Invitee, out var webSocketStruct))
        {
            // User not connected to websocket. Throw it out.
            return;
        }
        if (!Groups.Any(x => x.Owner == ticket.UserId && x.Id == groupKick.GroupId))
        {
            // send back we cant. (We not an owner)
            return;
        }
        webSocketStruct.SendWebSocketByteArray(data.ToArray());
        Groups.First(x => x.Owner == ticket.UserId).UserIds.Remove(groupKick.Invitee);
    }
}

