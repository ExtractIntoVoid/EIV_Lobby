using EIV_Common.Encryptions;
using LobbyLib.Jsons;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LobbyLib.CustomTicket;

public class TicketProcess
{
    static RsaService? Service;

    public static void Start()
    {
        var rsa = RSA.Create(2048);
        File.WriteAllText("rsa.xml", rsa.ToXmlString(true));
        rsa.FromXmlString(File.ReadAllText("rsa.xml"));
        Service = new RsaService(rsa);
    }

    public static string CreateTicket(UserData data)
    {
        if (Service == null)
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
        return Convert.ToHexString(Service!.Encrypt(Encoding.Default.GetBytes(ser)));
    }

    public static TicketStruct? GetTicket(string ticketEnc)
    {
        if (Service == null)
            Start();
        if (string.IsNullOrEmpty(ticketEnc))
            return null;

        try
        {
            var hexed = Convert.FromHexString(ticketEnc);
            var deced = Service!.Decrypt(hexed);
            var sered = Encoding.Default.GetString(deced);
            return JsonSerializer.Deserialize<TicketStruct>(sered);
        }
        catch
        {
            return null;
        }    
    }
}
