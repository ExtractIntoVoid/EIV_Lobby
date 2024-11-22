using EIV_Common;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using LobbyLib.Jsons;
using System.Security.Cryptography;

namespace LobbyLib.JWT;

internal class JWTHelper
{
    /// <summary>
    /// Get JWT Token as JSON
    /// </summary>
    /// <param name="token">The Token</param>
    /// <returns>JSON String</returns>
    public static string GetJWTJson(string token)
    {
        RSA rsa = JWTHandler.GetRSA();
        var json = JwtBuilder.Create()
                             .WithAlgorithm(new RS256Algorithm(rsa, rsa))
                             .Decode(token);

        return json;
    }


    public static string Create(UserData userData)
    {
        var now = DateTime.Now;
        RSA rsa = JWTHandler.GetRSA();
        var token = JwtBuilder.Create()
        .WithAlgorithm(new RS256Algorithm(rsa, rsa))
        .AddClaim("uid", userData.UserId)
        .AddClaim("name", userData.Name)
        .ExpirationTime(now.AddHours(1))
        .IssuedAt(now)
        .Issuer("EIV_Lobby")
        .Encode();

        return token;
    }


    /// <summary>
    /// Validating any jwt Token
    /// </summary>
    /// <param name="token">The Token</param>
    /// <returns>True | False</returns>
    public static bool Validate(string token)
    {
        RSA rsa = JWTHandler.GetRSA();
        try
        {
            var json = JwtBuilder.Create()
                                 .WithAlgorithm(new RS256Algorithm(rsa, rsa))
                                 .MustVerifySignature()
                                 .Decode(token);
        }
        catch (TokenNotYetValidException)
        {
            Console.WriteLine("Token is not valid yet");
            return false;
        }
        catch (TokenExpiredException)
        {
            Console.WriteLine("Token has expired");
            return false;
        }
        catch (SignatureVerificationException)
        {
            Console.WriteLine("Token has invalid signature");
            return false;
        }
        return true;
    }

    public static bool ValidateMasterServerToken(string token)
    {
        // get url form masterServer
        RSA rsa = RSA.Create();
        //rsa.ImportRSAPublicKey([], out _);
        try
        {
            var json = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(rsa))
                .Decode(token);
        }
        catch (TokenNotYetValidException)
        {
            Console.WriteLine("Token is not valid yet");
            return false;
        }
        catch (TokenExpiredException)
        {
            Console.WriteLine("Token has expired");
            return false;
        }
        return true;
    }
}
