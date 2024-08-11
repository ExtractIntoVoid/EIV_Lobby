using EIV_Common;
using LobbyLib.Database;
using LobbyLib.Modding;
using LobbyLib.Web;
using System.Diagnostics;

namespace LobbyLib
{
    public class MainControl
    {
        public static bool IsAlreadyQuited = false;
        public static string IP = "https://127.0.0.1:7777";
        public static string ip_port = "127.0.0.1:7777";
        public static IDatabase Database;

        /// <summary>
        /// Init the Server by Parameters
        /// </summary>
        /// <param name="Ip">Server IP</param>
        /// <param name="port">Server Port</param>
        /// <param name="LoadPlugin">Can Load Plugins</param>
        public static bool InitAll(string Ip, int port, bool ssl = false)
        {
            var sw = Stopwatch.StartNew();
            if (ssl)
            {
                string _ip_port = $"https://{Ip}:{port}";
                IP = _ip_port;
                ip_port = $"{Ip}:{port}";
                //CertHelper.Make(IPAddress.Parse(Ip), _ip_port);
            }
            else
            {
                string _ip_port = $"http://{Ip}:{port}";
                IP = _ip_port;
                ip_port = $"{Ip}:{port}";
            }
            ServerManager.Start(Ip, port, ssl);

            // ini check
            if (!File.Exists("Config.ini"))
            {
                File.WriteAllText("Config.ini", LobbyLib_Res.Config);
            }

            // DatabaseType
            var databaseType = ConfigINI.Read("Config.ini","Database", "DatabaseType");
            if (!int.TryParse(databaseType, out int db_Type))
            {
                return false;
            }
            switch (db_Type)
            {
                case 0:
                    Database = new JsonDatabase();
                    break;
                case 1:
                    Database = new LiteDB_Database();
                    break;
                case 2:
                    Database = new MySQL_Database();
                    break;
                default:
                    break;
            }
            Database.Create();

            // modding
            var enable_mods = ConfigINI.Read("Config.ini", "Mod", "EnableMods");
            if (!int.TryParse(enable_mods, out int i_enable_mods))
            {
                return false;
            }
            if (i_enable_mods == 1)
            {
                ModLoader.LoadMods();
            }

            return true;
        }

        /// <summary>
        /// Stopping the Server
        /// </summary>
        public static void Stop()
        {
            if (IsAlreadyQuited)
                return;
            ServerManager.Stop();
            IsAlreadyQuited = true;
        }
    }
}
