using IniParser;
using IniParser.Model;

namespace LobbyLib.INI
{
    public class ConfigIni
    {
        public static string Read(string name, string section)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Config.ini");
            return data[name][section];
        }
    }
}
