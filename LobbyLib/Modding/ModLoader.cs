using System.Reflection;
using EIV_DataPack;
using EIV_JsonLib;
using EIV_JsonLib.Json;
using EIV_Common;
using EIV_Common.JsonStuff;

namespace LobbyLib.Modding;

public class ModLoader
{
    public static Dictionary<string, ILobbyMod> Mods = [];
    public static Dictionary<string, List<string>> JsonMods = [];
    public static Dictionary<string, List<string>> ModsFiles = [];
    static bool IsLobbyModEnabled = true;

    public static void LoadMods()
    {
        var EnableLobbyMods = ConfigINI.Read("Config.ini","Mod", "EnableLobbyMods");
        if (!int.TryParse(EnableLobbyMods, out int i_EnableLobbyMods))
        {
            return;
        }
        if (i_EnableLobbyMods == 0)
        {
            IsLobbyModEnabled = false;
        }

        string currdir = Directory.GetCurrentDirectory();
        LoadPackedMods(currdir);
        var EnableLoadUnpackingMods = ConfigINI.Read("Config.ini", "Mod", "EnableLoadUnpackingMods");
        if (!int.TryParse(EnableLoadUnpackingMods, out int i_EnableLoadUnpackingMods))
        {
            return;
        }
        if (i_EnableLoadUnpackingMods == 1)
        {
            LoadUnpackedMods(currdir);
        }      
    }

    public static void UnloadMods()
    {
        foreach (var item in Mods)
        {
            item.Value.ShutDown();
        }
        JsonMods.Clear();
        Storage.Items.Clear();
    }



    static void LoadPackedMods(string currdir)
    {
        if (!Directory.Exists(Path.Combine(currdir, "Mods"))) { Directory.CreateDirectory(Path.Combine(currdir, "Mods")); }

        foreach (var packedmods in Directory.GetFiles("Mods"))
        {
            if (packedmods.Contains(".disabled"))
                continue;
            var creator = DatapackCreator.Read(packedmods);
            var reader = creator.GetReader()!;
            reader.ReadFileNames();
            ModsFiles.Add(packedmods.Replace("Mods" + Path.DirectorySeparatorChar, ""), reader.Pack.FileNames);
            //reader.Pack.FileNames.ForEach(Console.WriteLine);
            var deps = reader.Pack.FileNames.Where(x => x.Contains(".dll") && x.Contains("LobbyDependencies"));
            foreach (var item in deps)
            {
                AppDomain.CurrentDomain.Load(reader.GetFileData(item));
            }
            var lobbyMods = reader.Pack.FileNames.Where(x => x.Contains(".LobbyMod.dll"));
            foreach (var item in lobbyMods)
            {
                var ass = AppDomain.CurrentDomain.Load(reader.GetFileData(item));
                LoadJsonLibMod(ass);
                if (IsLobbyModEnabled)
                    LoadLobbyMod(ass);
            }
            LoadAssets_Pack(reader, packedmods);
            creator.Close();
        }
    }

    static void LoadUnpackedMods(string currdir)
    {
        if (!Directory.Exists(Path.Combine(currdir, "UnpackedMods"))) { Directory.CreateDirectory(Path.Combine(currdir, "UnpackedMods")); }

        foreach (var unpackedmods in Directory.GetDirectories("UnpackedMods"))
        {
            if (unpackedmods.Contains(".disabled"))
                continue;

            ModsFiles.Add(unpackedmods.Replace("UnpackedMods" + Path.DirectorySeparatorChar, "") + ".unpacked", Directory.GetFiles(unpackedmods, "*", SearchOption.AllDirectories).ToList());
            if (Directory.Exists(Path.Combine(unpackedmods, "LobbyDependencies")))
            {
                foreach (var unpacked_dependency in Directory.GetFiles(Path.Combine(unpackedmods, "LobbyDependencies"), "*.dll"))
                {
                    AppDomain.CurrentDomain.Load(unpacked_dependency);
                }
            }
            foreach (var LobbyMod in Directory.GetFiles(unpackedmods, "*.LobbyMod.dll"))
            {
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

        //Console.WriteLine("jsonLib converter added");
        CoreConverters.Converters.Add(jsonLib);
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
        if (!Directory.Exists(Path.Combine(Dir, "Assets", "Items")))
            return;

        foreach (var json in Directory.GetFiles(Path.Combine(Dir, "Assets", "Items"), "*.json", SearchOption.AllDirectories))
        {
            var item = ConvertHelper.ConvertFromString(File.ReadAllText(json));
            if (item != null)
            {
                bool ret = Storage.Items.TryAdd(item.BaseID, item);
                if (!ret)
                    continue;
                if (JsonMods.ContainsKey(Dir))
                {
                    JsonMods[Dir].Add(item.BaseID);
                    continue;
                }
                JsonMods.Add(Dir, new() { item.BaseID });
            }  
        }
    }

    static void LoadAssets_Pack(DataPackReader reader, string filename)
    {
        var items = reader.Pack.FileNames.Where(x=>x.Contains(".json") && x.Contains("Assets/Items"));
        foreach (var item in items)
        {
            var real_item = ConvertHelper.ConvertFromString(System.Text.Encoding.UTF8.GetString(reader.GetFileData(item)));
            if (real_item != null)
            {
                bool ret = Storage.Items.TryAdd(real_item.BaseID, real_item);
                if (!ret)
                    continue;
                if (JsonMods.ContainsKey(filename))
                {
                    JsonMods[filename].Add(real_item.BaseID);
                    continue;
                }
                JsonMods.Add(filename, new() { real_item.BaseID });
            }
        }
    }
}
