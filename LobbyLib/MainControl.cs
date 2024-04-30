using LobbyLib.Web;
using System.Diagnostics;

namespace LobbyLib
{
    public class MainControl
    {
        public static bool IsAlreadyQuited = false;
        public static string IP = "https://127.0.0.1:7777";
        public static string ip_port = "127.0.0.1:7777";

        /// <summary>
        /// Init the Server by Parameters
        /// </summary>
        /// <param name="Ip">Server IP</param>
        /// <param name="port">Server Port</param>
        /// <param name="LoadPlugin">Can Load Plugins</param>
        public static void InitAll(string Ip, int port, bool ssl = false)
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
