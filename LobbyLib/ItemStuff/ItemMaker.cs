﻿using JsonLib.Interfaces;

namespace LobbyLib.ItemStuff
{
    public class ItemMaker
    {
        public static Dictionary<string, IItem> Items = [];

        public static IItem? MakeNewItem(string BaseId)
        {
            if (!Items.TryGetValue(BaseId, out IItem? item))
                return null;
            if (item == null)
                return null;
            return (IItem)item.Clone();
        }

        public static void PrintBaseIds()
        {
            foreach (var item in Items)
            {
                Console.WriteLine(item.Key);
            }

        }
    }
}
