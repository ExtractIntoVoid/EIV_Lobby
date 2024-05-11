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

namespace LobbyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Test(CompressionType.Deflate);
            /*
            Test(CompressionType.None);
            Console.WriteLine("\n");
            Thread.Sleep(10);
            Test(CompressionType.Deflate);
            Console.WriteLine("\n");
            Thread.Sleep(10);
            Test(CompressionType.GZip);
            Console.WriteLine("\n");
            Thread.Sleep(10);
            Test(CompressionType.ZLib);
            Console.WriteLine("\n");
            Thread.Sleep(10);
            Test(CompressionType.Brotli);
            Console.WriteLine("\n");
            Thread.Sleep(10);
            */
        }

        static void Test(CompressionType compressionType)
        {
            Console.WriteLine("Compression Type: " + compressionType);
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var data = DatapackCreator.Create($"test_{compressionType.ToString()}.eivp", compressionType);
            stopwatch.Stop();
            Console.WriteLine("Creating Time: " + stopwatch.ToString());
            stopwatch.Restart();
            stopwatch.Start();
            var writer = data.GetWriter();
            writer.AddDirectory("UnpackedMods", true);
            stopwatch.Stop();
            Console.WriteLine("Adding dir Time: " + stopwatch.ToString());
            stopwatch.Restart();
            stopwatch.Start();
            writer.Save();

            writer.AddDirectory("test", true);
            writer.Save();

            stopwatch.Stop();
            Console.WriteLine("Save Time: " + stopwatch.ToString());
            stopwatch.Restart();
            data.Close();
            data = DatapackCreator.Read($"test_{compressionType.ToString()}.eivp");
            var reader = data.GetReader();
            Console.WriteLine("Filenames: " + reader.FileNameCount);
            stopwatch.Start();
            var filedata = reader.GetFileData(reader.FileNameCount - 1);
            stopwatch.Stop();
            Console.WriteLine("GetFileData No Reading Time: " + stopwatch.ToString());
            File.WriteAllBytes("last_filedata.txt", filedata);
            stopwatch.Start();
            reader.ReadFileNames();
            File.WriteAllText($"Files_{compressionType.ToString()}.txt", string.Join("\n", reader.Pack.FileNames));
            Console.WriteLine("ReadFileNames: " + stopwatch.ToString());
            filedata = reader.GetFileData("Core\\Core.JsonLib.Lobby.dll");
            stopwatch.Stop();
            Console.WriteLine("GetFileData last: " + stopwatch.ToString());
            File.WriteAllBytes("Core.JsonLib.Lobby.dll_new", filedata);
            data.Close();
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