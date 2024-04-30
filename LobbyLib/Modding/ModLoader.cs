using JsonLib.Convert;
using JsonLib.Modding;
using LobbyLib.ItemStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LobbyLib.Modding
{
    public class ModLoader
    {

        public static void LoadMods()
        {
            string currdir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currdir, "UnpackedMods"))) { Directory.CreateDirectory(Path.Combine(currdir, "UnpackedMods")); }

            foreach (var unpackedmods in Directory.GetDirectories("UnpackedMods"))
            {
                Console.WriteLine(unpackedmods);
                foreach (var unpacked_dependency in Directory.GetFiles(Path.Combine(unpackedmods, "Dependencies"), "*.dll"))
                {
                    Console.WriteLine(unpacked_dependency);
                    AppDomain.CurrentDomain.Load(unpacked_dependency);
                }
                foreach (var lobbyMod in Directory.GetFiles(unpackedmods, "*.Lobby.dll"))
                {
                    Console.WriteLine(lobbyMod);
                    var ass = AppDomain.CurrentDomain.Load(File.ReadAllBytes(lobbyMod));
                    LoadJsonLibMod(ass);
                }
                LoadAssets(unpackedmods);
            }

        }

        static void LoadJsonLibMod(Assembly assembly)
        {
            if (assembly.GetType("JsonLib_Mod.JsonLibConvert") == null)
                return;

            var jsonLib = (IJsonLibConverter?)Activator.CreateInstance(assembly.GetType("JsonLib_Mod.JsonLibConvert")!);
            if (jsonLib == null)
                return;

            Console.WriteLine("jsonLib converter added");
            JsonLib.JsonLibConverters.ModdedConverters.Add(jsonLib);
        }

        static void LoadAssets(string Dir)
        {
            foreach (var json in Directory.GetFiles(Path.Combine(Dir, "Assets", "Items"), "*.json", SearchOption.AllDirectories))
            {
                Console.WriteLine(json);
                var item = ConvertHelper.ConvertFromString(File.ReadAllText(json));
                if (item != null)
                {
                    ItemMaker.Items.Add(item.BaseID, item);
                }
                
            }
        }
    }
}
