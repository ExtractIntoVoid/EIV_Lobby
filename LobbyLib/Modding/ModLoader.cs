using JsonLib.Convert;
using JsonLib.Modding;
using LobbyLib.INI;
using LobbyLib.ItemStuff;
using System.Reflection;

namespace LobbyLib.Modding
{
    public class ModLoader
    {
        public static Dictionary<string, ILobbyMod> Mods = new();
        public static Dictionary<string, string> JsonMods = new();
        static bool IsLobbyModEnabled = true;

        public static void LoadMods()
        {
            string currdir = Directory.GetCurrentDirectory();
            LoadPackedMods(currdir);
            var EnableLoadUnpackingMods = ConfigIni.Read("Mod", "EnableLoadUnpackingMods");
            if (!int.TryParse(EnableLoadUnpackingMods, out int i_EnableLoadUnpackingMods))
            {
                return;
            }
            if (i_EnableLoadUnpackingMods == 1)
            {
                LoadUnpackedMods(currdir);
            }

            var EnableLobbyMods = ConfigIni.Read("Mod", "EnableLobbyMods");
            if (!int.TryParse(EnableLobbyMods, out int i_EnableLobbyMods))
            {
                return;
            }
            if (i_EnableLobbyMods == 0)
            {
                IsLobbyModEnabled = false;
            }
        }

        public static void UnloadMods()
        {
            foreach (var item in Mods)
            {
                item.Value.ShutDown();
            }
            foreach (var item in JsonMods)
            {
                ItemMaker.Items.Remove(item.Value);
            }
        }



        static void LoadPackedMods(string currdir)
        {
            if (!Directory.Exists(Path.Combine(currdir, "Mods"))) { Directory.CreateDirectory(Path.Combine(currdir, "Mods")); }
        }

        static void LoadUnpackedMods(string currdir)
        {
            if (!Directory.Exists(Path.Combine(currdir, "UnpackedMods"))) { Directory.CreateDirectory(Path.Combine(currdir, "UnpackedMods")); }

            foreach (var unpackedmods in Directory.GetDirectories("UnpackedMods"))
            {
                Console.WriteLine(unpackedmods);
                foreach (var unpacked_dependency in Directory.GetFiles(Path.Combine(unpackedmods, "Dependencies"), "*.dll"))
                {
                    Console.WriteLine(unpacked_dependency);
                    AppDomain.CurrentDomain.Load(unpacked_dependency);
                }
                foreach (var LobbyMod in Directory.GetFiles(unpackedmods, "*.LobbyMod.dll"))
                {
                    Console.WriteLine(LobbyMod);
                    var ass = AppDomain.CurrentDomain.Load(File.ReadAllBytes(LobbyMod));
                    LoadJsonLibMod(ass);
                    if (IsLobbyModEnabled)
                        LoadLobbyMod(ass);
                }
                LoadAssets_Unpack(unpackedmods);
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

        static void LoadLobbyMod(Assembly assembly)
        {
            if (string.IsNullOrEmpty(assembly.FullName))
                return;

            if (assembly.GetType("LobbyMod.LobbyMod") == null)
                return;

            var jsonLib = (ILobbyMod?)Activator.CreateInstance(assembly.GetType("LobbyMod.LobbyMod")!);
            if (jsonLib == null)
                return;
            jsonLib.Initialize();
            Mods.Add(assembly.FullName, jsonLib);
        }

        static void LoadAssets_Unpack(string Dir)
        {
            foreach (var json in Directory.GetFiles(Path.Combine(Dir, "Assets", "Items"), "*.json", SearchOption.AllDirectories))
            {
                var item = ConvertHelper.ConvertFromString(File.ReadAllText(json));
                if (item != null)
                {
                    ItemMaker.Items.Add(item.BaseID, item);
                    JsonMods.Add(Dir, item.BaseID);
                }
                
            }
        }
    }
}
