using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using LobbyLib.Connection;
using LobbyLib.INI;
using LobbyLib.Jsons;
using System.Security.Cryptography;

namespace LobbyLib.JWT
{
    internal class JWTHelper
    {
        /// <summary>
        /// Get JWT Token as JSON
        /// </summary>
        /// <param name="token">The Token</param>
        /// <returns>JSON String</returns>
        public static string GetJWTJson(string token)
        {
            RSA rsa = Encryption.GetServerKey();
            var json = JwtBuilder.Create()
                                 .WithAlgorithm(new RS256Algorithm(rsa, rsa))
                                 .Decode(token);

            return json;
        }


        public static string Create(UserData userData)
        {
            var now = DateTime.Now;
            RSA rsa = Encryption.GetServerKey();
            var token = JwtBuilder.Create()
            .WithAlgorithm(new RS256Algorithm(rsa, rsa))
            .AddClaim("uid", userData.UserId)
            .AddClaim("name", userData.Name)
            .AddClaim<Badge>("badge", new()
            { 
                ColorHex = ConfigIni.Read("Badges", "DefaultColor"),
                PlayerId = userData.UserId,
                Title = ConfigIni.Read("Badges", "DefaultTitle"),
            })
            .ExpirationTime(now.AddHours(1))
            .IssuedAt(now)
            .Issuer("EIV_Lobby")
            .Encode();

            return token;
        }

        public static string Create(UserData userData, Badge badge)
        {
            var now = DateTime.Now;
            RSA rsa = Encryption.GetServerKey();
            var token = JwtBuilder.Create()
            .WithAlgorithm(new RS256Algorithm(rsa, rsa))
            .AddClaim("uid", userData.UserId)
            .AddClaim("name", userData.Name)
            .AddClaim<Badge>("badge", badge)
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
            RSA rsa = Encryption.GetServerKey();
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
            RSA rsa = Encryption.GetServerKey();
            try
            {
                var json = JwtBuilder.Create()
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
}
