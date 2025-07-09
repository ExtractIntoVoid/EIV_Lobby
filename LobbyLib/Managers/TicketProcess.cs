using LobbyLib.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LobbyLib.Managers;

public class TicketManager
{
    const string RSAXML = "rsa.xml";
    static RSA Rsa;

    static TicketManager()
    {
        Rsa = RSA.Create(2048);
        if (!File.Exists(RSAXML))
            File.WriteAllText(RSAXML, Rsa.ToXmlString(true));
        Rsa.FromXmlString(File.ReadAllText(RSAXML));
    }

    public static string CreateTicket(UserData data)
    {
        var ser = JsonSerializer.Serialize(new TicketModel()
        { 
            CreatedTime = DateTime.Now,
            Id = data.Id,
            Name = data.Name,
            Permission = 0,
            UserId = data.UserId,
        });
        var bytes = Encoding.Default.GetBytes(ser);
        var enced = Rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        return Convert.ToHexString(enced);
    }

    public static TicketModel? GetTicket(string ticketEnc)
    {
        if (string.IsNullOrEmpty(ticketEnc))
            return null;

        try
        {
            var hexed = Convert.FromHexString(ticketEnc);
            var deced = Rsa.Decrypt(hexed, RSAEncryptionPadding.Pkcs1);
            var sered = Encoding.Default.GetString(deced);
            return JsonSerializer.Deserialize<TicketModel>(sered);
        }
        catch
        {
            return null;
        }    
    }
}
