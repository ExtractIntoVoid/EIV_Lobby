using ModdableWebServer.Attributes;
using ModdableWebServer;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace LobbyLib.Web
{
    public class ChatWebsocket
    {
        [WS("/Socket/Chat")]
        public static void WSControl(WebSocketStruct socketStruct)
        {
            var jwt = socketStruct.Request.Headers["authorization"];
            Console.WriteLine("websocket hit!");

            if (socketStruct.WSRequest != null)
            {
                Control(socketStruct.WSRequest.Value.buffer, socketStruct.WSRequest.Value.offset, socketStruct.WSRequest.Value.size, socketStruct);
            }
        }

        public static void Control(byte[] buffer, long offset, long size, WebSocketStruct socketStruct)
        {
            if (size == 0)
                return;
            buffer = buffer.Take((int)size).ToArray();

            var str = Encoding.UTF8.GetString(buffer);
           


        }

    }
}
