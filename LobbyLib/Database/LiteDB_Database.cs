using LobbyLib.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyLib.Database
{
    internal class LiteDB_Database : IDatabase
    {
        public void AddChat(ChatMessage chat)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void DeleteBadge(string UserId)
        {
            throw new NotImplementedException();
        }

        public void DeleteChat(string ReceiverId)
        {
            throw new NotImplementedException();
        }

        public void DeleteInventory(Guid Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteStashInventory(Guid Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserData(string UserId)
        {
            throw new NotImplementedException();
        }

        public ChatMessage? GetChat(string ReceiverId, ulong MessageId)
        {
            throw new NotImplementedException();
        }

        public List<ChatMessage> GetChats(string ReceiverId)
        {
            throw new NotImplementedException();
        }

        public Inventory? GetInventory(Guid Id)
        {
            throw new NotImplementedException();
        }

        public StashInventory? GetStashInventory(Guid Id)
        {
            throw new NotImplementedException();
        }

        public UserData? GetUserData(string UserId)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void SaveInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public void SaveStashInventory(StashInventory inventory)
        {
            throw new NotImplementedException();
        }

        public void SaveUserData(UserData userData)
        {
            throw new NotImplementedException();
        }
    }
}
