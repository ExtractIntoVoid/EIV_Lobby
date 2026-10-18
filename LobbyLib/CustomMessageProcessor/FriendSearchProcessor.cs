using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Managers;
using NetCoreServer;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="FriendSearchRequest"/>.
/// </summary>
public class FriendSearchProcessor : BaseProcessor<FriendSearchRequest>
{
    /// <inheritdoc/>
    protected override bool OnProcess([NotNull] FriendSearchRequest message)
    {
        Collection<Guid> users = [];

        string search = message.Search;
        bool searchId = message.Search.StartsWith("i:", StringComparison.InvariantCultureIgnoreCase);
        bool searchCustomUserId = message.Search.StartsWith("u:", StringComparison.InvariantCultureIgnoreCase);

        if (searchId)
        {
            string searchWithoutPrefix = search.RemoveSuffix("i: ");
            users = UserManager.GetUsersGuid(x => x.Id.ToString() == searchWithoutPrefix);
        }
        else if (searchCustomUserId)
        {
            string searchWithoutPrefix = search.RemoveSuffix("u: ");
            users = UserManager.GetUsersGuid(x => x.CustomId.Contains(searchWithoutPrefix, StringComparison.InvariantCultureIgnoreCase));
        }
        else
        {
            users = UserManager.GetUsersGuid(x => x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
        }

        FriendSearchResponse searchResponse = new(users);
        return SendResponse(searchResponse);
    }
}
