using IniParser;
using IniParser.Model;

namespace LobbyLib.INI;

/// <summary>
/// Provides ini related settinsg.
/// </summary>
[GenerateIni(typeof(MainIni))]
public static partial class IniGeneratedContext
{
    private static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

    /// <summary>
    /// Gets the data parser for files.
    /// </summary>
    public static FileIniDataParser DataParser { get; private set; } = new();

    /// <summary>
    /// Get the ini data from <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The file.</param>
    /// <returns>The ini data.</returns>
    public static IniData GetIni(string filename)
    {
        return DataParser.ReadFile(filename, Encoding);
    }

    /// <summary>
    /// Saves <paramref name="data"/> to <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The file.</param>
    /// <param name="data">The ini data.</param>
    public static void SaveIni(string filename, IniData data)
    {
        DataParser.WriteFile(filename, data, Encoding);
    }
}
