using ModdableWebServer.Attributes;
using ModdableWebServer;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using ModdableWebServer.Helper;
using System.Security.Cryptography;

namespace LobbyLib.Web
{
    public class ChatWebsocket
    {
        [WS("/Socket/Chat")]
        public static void WSControl(WebSocketStruct socketStruct)
        {
            if (!socketStruct.Request.Headers.TryGetValue("authorization", out var jwt))
            {
                socketStruct.SendWebSocketClose(401, "authorization is not found!");
                return;
            }

            var body = socketStruct.Request.Body; // body must be the RSA public key when joining!
            if (body.Contains("RSAKeyValue") && body.Contains("RSAKeyValue") && body.Contains("Modulus") && body.Contains("Exponent") && !body.Contains("<P>"))
            {
                try
                {
                    RSA rsatest = RSA.Create();
                    rsatest.FromXmlString(body);
                }
                catch (Exception)
                {
                    socketStruct.SendWebSocketClose(401, "Your Body is not contains wrong RSA Public Key!");
                    return;
                }
                
            }
            else
            {
                socketStruct.SendWebSocketClose(401, "Your Body is not contains your RSA Public Key!");
                return;
            }

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
