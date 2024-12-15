using System.Reflection;
using EIV_DataPack;
using EIV_Common;
using EIV_Common.JsonStuff;
using ModAPI;

namespace LobbyLib.Modding;

public class ModLoader
{
    public static Dictionary<string, ILobbyMod> LobbyMods = [];
    static bool IsLobbyModEnabled = true;

    public static void LoadMods()
    {
        IsLobbyModEnabled = ConfigINI.Read<bool>("Config.ini","Mod", "EnableLobbyMods");
        if (!IsLobbyModEnabled)
            return;
        ModManager.Init();
        string currdir = Directory.GetCurrentDirectory();
        LoadPackedMods(currdir);
        var EnableLoadUnpackingMods = ConfigINI.Read<bool>("Config.ini", "Mod", "EnableLoadUnpackingMods");
        if (EnableLoadUnpackingMods)
        {
            LoadUnpackedMods(currdir);
        }      
    }

    public static void UnloadMods()
    {
        ModManager.DeInit();
        foreach (var item in LobbyMods)
        {
            item.Value.ShutDown();
        }
        LobbyMods.Clear();
        Storage.ClearAll();
    }

    static void LoadPackedMods(string currdir)
    {
        if (!Directory.Exists(Path.Combine(currdir, "Mods")))
            Directory.CreateDirectory(Path.Combine(currdir, "Mods"));

        foreach (var packedmods in Directory.GetFiles("Mods"))
        {
            if (packedmods.Contains("disabled") || packedmods.Contains(".d"))
                continue;

            var creator = DatapackCreator.Read(packedmods);
            var reader = creator.GetReader()!;
            reader.ReadFileNames();
            ModManager.LoadAssets_Pack(reader);

            foreach (var item in reader.Pack.FileNames.Where(x => x.Contains(".dll") && x.Contains("LobbyDependencies")))
                MainLoader.MainLoadContext.LoadFromStream(new MemoryStream(reader.GetFileData(item)));

            foreach (var item in reader.Pack.FileNames.Where(x => x.Contains("*.dll")))
            {
                var asm = MainLoader.MainLoadContext.LoadFromStream(new MemoryStream(reader.GetFileData(item)));
                MainLoader.LoadMod(asm);
                ModManager.LoadMod_JsonLib(asm);
                if (IsLobbyModEnabled)
                    LoadLobbyMod(asm);
            }
            creator.Close();
        }
    }

    static void LoadUnpackedMods(string currdir)
    {
        if (!Directory.Exists(Path.Combine(currdir, "UnpackedMods")))
            Directory.CreateDirectory(Path.Combine(currdir, "UnpackedMods"));

        foreach (var unpackedmods in Directory.GetDirectories("UnpackedMods"))
        {
            // Disabled Unpacked Mods.
            if (unpackedmods.Contains("disabled") || unpackedmods.Contains(".d"))
                continue;

            ModManager.LoadAssets_Unpack(unpackedmods);
            // Dependencies for Lobby.
            MainLoader.LoadDependencies(Path.Combine(unpackedmods, "LobbyDependencies"));
            MainLoader.LoadModInCustomDirectory(unpackedmods);

            foreach (var LobbyMod in MainLoader.Mods)
                ModManager.LoadMod_JsonLib(LobbyMod);

            foreach (var LobbyMod in MainLoader.Mods.Where(x => x.FullName != null && x.FullName.Contains("Lobby")).ToList())
            {
                if (IsLobbyModEnabled)
                    LoadLobbyMod(LobbyMod);
            }
        }
    }

    public static void LoadLobbyMod(Assembly assembly)
    {
        ModManager.LoadMod(typeof(ILobbyMod), assembly, Delegate);
        void Delegate(Type? retType, object? obj)
        {
            if (obj == null)
                return;
            ILobbyMod? lobbyMod = (ILobbyMod?)obj;
            if (lobbyMod != null)
            {
                lobbyMod.Initialize();
                LobbyMods.Add(assembly.FullName!, lobbyMod);
            }
        }
    }
}
