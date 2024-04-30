using JsonLib.Modding;
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
    }
}
