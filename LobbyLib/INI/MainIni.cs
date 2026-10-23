using IniParser.Model;

namespace LobbyLib.INI;

/// <summary>
/// Represent a main ini configuration.
/// </summary>
public class MainIni
{
    /// <summary>
    /// Gets the main ini instance.
    /// </summary>
    public static MainIni Instance
    {
        get
        {
            if (field == null)
            {
                field = new();
                const string ConfigIniName = "Config.ini";
                if (!File.Exists(ConfigIniName))
                {
                    IniData writtenData = IniGeneratedContext.WriteMainIniIniData(field);
                    IniGeneratedContext.SaveIni(ConfigIniName, writtenData);
                }
                else
                {
                    IniData data = IniGeneratedContext.GetIni(ConfigIniName);
                    IniGeneratedContext.ReadMainIniIniData(field, data);
                }
            }

            return field;
        }
    }

    /// <summary>
    /// Gets or sets the federation configuration.
    /// </summary>
    public FederationConfiguration Federation { get; set; } = new();

    /// <summary>
    /// Gets or sets the lobby configuration.
    /// </summary>
    public LobbyConfiguration Lobby { get; set; } = new();

    /// <summary>
    /// Gets or sets the ssl configuration.
    /// </summary>
    public SSLConfiguration SSL { get; set; } = new();

    /// <summary>
    /// Gets or sets the game data configuration.
    /// </summary>
    public GameDataConfiguration GameData { get; set; } = new();

    /// <summary>
    /// Gets or sets the game server configuration.
    /// </summary>
    public GameServerConfiguration GameServer { get; set; } = new();

    /// <summary>
    /// Gets or sets the server to server configuration.
    /// </summary>
    public Server2ServerConfiguration Server2Server { get; set; } = new();
}
