using EIV_Common;
using System.Security.Cryptography;
using System.Text;

namespace LobbyLib.Connection
{
    public class Encryption
    {
        public static string DecryptMessage(string message, RSA pubkey)
        {
            return Encoding.UTF8.GetString(pubkey.Decrypt(Convert.FromHexString(message), RSAEncryptionPadding.Pkcs1));
        }

        public static string EncryptMessage(string message, RSA serverkey)
        {
            return Convert.ToHexString(serverkey.Encrypt(Encoding.UTF8.GetBytes(message), RSAEncryptionPadding.Pkcs1));
        }

        public static void CreateServerKey()
        {
            var rsa = RSA.Create(1024);
            ConfigINI.Write("Config.ini","EncKey", "RSAKey" , rsa.ToXmlString(true));
        }

        public static RSA GetServerKey()
        {
            var rsa = RSA.Create(1024);
            string xml = ConfigINI.Read("Config.ini", "EncKey", "RSAKey");
            if (string.IsNullOrEmpty(xml))
            {
                ConfigINI.Write("Config.ini", "EncKey", "RSAKey", rsa.ToXmlString(true));
                return rsa;
            }
            rsa.FromXmlString(xml);
            return rsa;
        }
    }
}
