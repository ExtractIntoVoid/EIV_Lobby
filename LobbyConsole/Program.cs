using JsonLib;
using JsonLib.Convert;
using JsonLib.Interfaces;
using LobbyLib.Database;
using LobbyLib.ItemStuff;
using LobbyLib.Jsons;
using LobbyLib.Modding;
using Newtonsoft.Json;

namespace LobbyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            ModLoader.LoadMods();

            ItemMaker.PrintBaseIds();

            JsonDatabase json = new();

            var id = Guid.NewGuid();
            Console.WriteLine("id!" + id);

            Inventory inventory = new Inventory()
            { 
                HoldingItem = "Healing_MedKit",
                BackSlot = "Backpack_64",
                PocketItems = new(),
                MeleeSlot = "Melee_Knife",
                SecondarySlot = new()
                { 
                    Id = "Gun_Pistol",
                    MagazineId = "Magazine_919",
                    Ammos = new() 
                    { 
                        "Ammo_919", "Ammo_919","Ammo_919","Ammo_919", "Ammo_919_AP",
                    }
                },
                RigSlot = null,
                UserId = id
            };

            json.SaveInventory(inventory);

            var inv = json.GetInventory(id);
            Console.WriteLine(inv);
        }
    }
}
