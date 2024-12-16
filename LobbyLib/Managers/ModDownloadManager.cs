﻿namespace LobbyLib.Managers;

public class ModDownloadManager
{
    public static string GetModsTxt(string build)
    {
        if (string.IsNullOrEmpty(build))
            return string.Empty;

        // we skip if we dont recoginse it
        if (build != "client" && build != "server" && build != "game")
            return string.Empty;

        if (!Directory.Exists($"{build}_mods"))
            return string.Empty;

        var files = Directory.GetFiles($"{build}_mods", "*.*", SearchOption.AllDirectories);
        List<string> modFilesSimple = [];

        foreach (string file in files)
        {
            // filter disabled files, directories.
            if (file.Contains(".d") || file.Contains(".disabled"))
                continue;
            modFilesSimple.Add(file.Replace(Path.Combine(Directory.GetCurrentDirectory(),$"{build}_mods"), ""));
        }
        return string.Join("\n", modFilesSimple);
    }

    public static byte[] GetFile(string build, string file)
    {
        if (string.IsNullOrEmpty(build) || string.IsNullOrEmpty(file))
            return Array.Empty<byte>();

        // we skip if we dont recoginse it
        if (build != "client" && build != "server" && build != "game")
            return Array.Empty<byte>();
        var modfile = Path.Combine(Directory.GetCurrentDirectory(), $"{build}_mods", file);
        if (!File.Exists(modfile))
            return Array.Empty<byte>();
        return File.ReadAllBytes(modfile);
    }
}
