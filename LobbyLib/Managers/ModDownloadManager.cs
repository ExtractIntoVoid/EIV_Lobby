namespace LobbyLib.Managers;

/// <summary>
/// Provides a simple way of getting the mods.
/// </summary>
public static class ModDownloadManager
{
    private static readonly List<string> BuildNames = ["client", "server", "game"];

    /// <summary>
    /// Get the mods txt from <paramref name="build"/>.
    /// </summary>
    /// <param name="build">The build to get the mods from.</param>
    /// <returns>The mods.</returns>
    public static string GetModsTxt(string build)
    {
        if (string.IsNullOrEmpty(build))
        {
            return string.Empty;
        }

        // we skip if we dont recoginse it
        if (!BuildNames.Contains(build))
        {
            return string.Empty;
        }

        if (!Directory.Exists($"{build}_mods"))
        {
            return string.Empty;
        }

        var files = Directory.GetFiles($"{build}_mods", "*.*", SearchOption.AllDirectories);
        List<string> modFilesSimple = [];

        foreach (string file in files)
        {
            // filter disabled files, directories.
            if (file.Contains(".d", StringComparison.InvariantCultureIgnoreCase) || file.Contains(".disabled", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            modFilesSimple.Add(file.Replace(Path.Combine(Directory.GetCurrentDirectory(), $"{build}_mods"), string.Empty, StringComparison.InvariantCultureIgnoreCase));
        }

        return string.Join("\n", modFilesSimple);
    }

    /// <summary>
    /// Get the file data from mods.
    /// </summary>
    /// <param name="build">The build.</param>
    /// <param name="file">The file name.</param>
    /// <returns>The read data.</returns>
    public static byte[] GetFile(string build, string file)
    {
        if (string.IsNullOrEmpty(build) || string.IsNullOrEmpty(file))
        {
            return [];
        }

        // we skip if we dont recoginse it
        if (!BuildNames.Contains(build))
        {
            return [];
        }

        // TODO: Validate file to not be traversal path!
        var modfile = Path.Combine(Directory.GetCurrentDirectory(), $"{build}_mods", file);
        if (!File.Exists(modfile))
        {
            return [];
        }

        return File.ReadAllBytes(modfile);
    }
}
