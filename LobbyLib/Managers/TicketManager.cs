using EIV_LobbyData;
using LobbyLib.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LobbyLib.Managers;

/// <summary>
/// Provides managing ticket.
/// </summary>
public static class TicketManager
{
    private const string RSAXML = "rsa.xml";

    private static RSA MyRSA
    {
        get
        {
            if (field == null)
            {
                field = RSA.Create(2048);
                if (!File.Exists(RSAXML))
                {
                    File.WriteAllText(RSAXML, field.ToXmlString(true));
                }

                field.FromXmlString(File.ReadAllText(RSAXML));
            }

            return field;
        }
    }

    /// <summary>
    /// Create an ecrypted ticket from <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>The ecrypted ticket.</returns>
    public static string CreateTicket(UserData data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<TicketModel>(out var typeInfo))
        {
            return string.Empty;
        }

        var ser = JsonSerializer.Serialize(
            new TicketModel()
            {
                CreatedTime = DateTime.Now,
                Id = data.Id,
                Name = data.Name,
                UserId = data.CustomId,
            },
            typeInfo);
        var bytes = Encoding.Default.GetBytes(ser);
        var enced = MyRSA.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        return Convert.ToHexString(enced);
    }

    /// <summary>
    /// Gets the ticket from encrypted input.
    /// </summary>
    /// <param name="ticketEnc">Encrypted input.</param>
    /// <returns>The ticket.</returns>
    public static TicketModel? GetTicket(string ticketEnc)
    {
        if (string.IsNullOrEmpty(ticketEnc))
        {
            return null;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<TicketModel>(out var typeInfo))
        {
            return null;
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var hexed = Convert.FromHexString(ticketEnc);
            var deced = MyRSA.Decrypt(hexed, RSAEncryptionPadding.Pkcs1);
            var sered = Encoding.Default.GetString(deced);
            return JsonSerializer.Deserialize(sered, typeInfo);
        }
        catch (Exception)
        {
            return null;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
