using LobbyLib.Jsons;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LobbyLib.CustomTicket;

public class TicketProcess
{
    static RSA? Rsa;

    public static void Start()
    {
        Rsa = RSA.Create(2048);
        if (!File.Exists("rsa.xml"))
            File.WriteAllText("rsa.xml", Rsa.ToXmlString(true));
        Rsa.FromXmlString(File.ReadAllText("rsa.xml"));
    }

    public static string CreateTicket(UserData data)
    {
        if (Rsa == null)
            Start();
        TicketStruct ticketStruct = new()
        { 
            CreatedTime = DateTime.Now,
            Id = data.Id,
            Name = data.Name,
            Permission = 0,
            UserId = data.UserId,
        };
        
        var ser = JsonSerializer.Serialize(ticketStruct);
        return Convert.ToHexString(Rsa!.Encrypt(Encoding.Default.GetBytes(ser), RSAEncryptionPadding.Pkcs1));
    }

    public static TicketStruct? GetTicket(string ticketEnc)
    {
        if (Rsa == null)
            Start();
        if (string.IsNullOrEmpty(ticketEnc))
            return null;

        try
        {
            var hexed = Convert.FromHexString(ticketEnc);
            var deced = Rsa!.Decrypt(hexed, RSAEncryptionPadding.Pkcs1);
            var sered = Encoding.Default.GetString(deced);
            return JsonSerializer.Deserialize<TicketStruct>(sered);
        }
        catch
        {
            return null;
        }    
    }
}
