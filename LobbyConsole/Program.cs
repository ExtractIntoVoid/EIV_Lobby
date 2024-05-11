using JsonLib;
using JsonLib.Convert;
using JsonLib.Interfaces;
using LobbyLib.Database;
using LobbyLib.ItemStuff;
using LobbyLib.Jsons;
using LobbyLib.Modding;
using Newtonsoft.Json;
using EIV_DataPack;
using System.Diagnostics;
using LobbyLib;

namespace LobbyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //CompressCore();
            //return;
            Console.WriteLine(MainControl.InitAll("0.0.0.0", 6969));
            foreach (var item in ModLoader.Mods.Keys)
            {
                Console.WriteLine(item);
            }
            foreach (var item in ModLoader.JsonMods.Keys)
            {
                Console.WriteLine(item);
            }
            string ret = "ret";
            while (ret != "q")
            {
                ret = Console.ReadLine().ToLower();
                if (ret == "test")
                {
                    
                }
            }
            MainControl.Stop();
        }


        static void CompressCore()
        {
            var data = DatapackCreator.Create($"Core.eivp");
            var writer = data.GetWriter()!;
            writer.OnFileAdded += Writer_OnFileAdded;
            writer.AddDirectory(Path.Combine("UnpackedMods", "Core"), true);
            writer.OnFileAdded -= Writer_OnFileAdded;
            writer.Save();
            writer.Close();
        }

        private static void Writer_OnFileAdded(string Filename)
        {
            Console.WriteLine(Filename);
        }
    }
}

/*
 
 
 
 
 
 
 
            var eivp = "eivp"u8.ToArray();
            int MagicInt = 1886808421;

            Console.WriteLine(BitConverter.ToString(eivp));
            Console.WriteLine(Convert.ToHexString(eivp));
            Console.WriteLine(BitConverter.ToInt32(eivp));
            Console.WriteLine(MagicInt);
            return;

            ModLoader.LoadMods();

            ItemMaker.PrintBaseIds();

            var x = JsonConvert.DeserializeObject<List<ItemRecreator>>(File.ReadAllText("DefaultItems.json"));

            var items = ItemRemake.ItemRemaker(x);
            Console.WriteLine("Items reconstructed!");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            File.WriteAllText("reconst.json",JsonConvert.SerializeObject(items, Formatting.Indented));
 
 
 
 */
/*
           JsonDatabase json = new();

           var id = Guid.NewGuid();
           Console.WriteLine("id!" + id);

           Inventory inventory = new Inventory()
           { 
               HoldingItem = ItemMaker.MakeNewItem("Healing_MedKit"),
               BackSlotId = "Backpack_64",
               PocketItem = new(),
               MeleeSlot = ItemMaker.CreateItem<IMelee>("Melee_Knife"),
               SecondarySlot = new()
               { 
                   Gun = ItemMaker.CreateItem<IGun>("Gun_Pistol")
               },
               RigSlot = null,
               UserId = id
           };

           var rig = ItemMaker.CreateItem<IRig>("Rig_Small");
           rig.PlateSlotId = "ArmorPlate_Lightweight";
           inventory.RigSlot = rig;

           Console.WriteLine("919: " + inventory.SecondarySlot.Gun.TryInsertMagazine("Magazine_919", "Ammo_919", 3));

           Console.WriteLine("919 ap: " + inventory.SecondarySlot.Gun.Magazine.TryInsertAmmos("Ammo_919_AP", 2));

           Console.WriteLine("556: " + inventory.SecondarySlot.Gun.Magazine.TryInsertAmmos("Ammo_556", 1));

           json.SaveInventory(inventory);

           var inv = json.GetInventory(id);
           Console.WriteLine(inv);
           */