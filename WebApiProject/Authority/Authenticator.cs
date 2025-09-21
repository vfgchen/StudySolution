using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApiProject.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRepository.GetApplicationByClientId(clientId);
            if (app == null) return false;
            return app.ClientId == clientId && app.Secret == secret;
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string securityKey)
        {
            // Signing Key

            // Algorithm
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                SecurityAlgorithms.HmacSha256Signature
                );

            // Payload (cliams)
            var app = AppRepository.GetApplicationByClientId(clientId);
            var cliams = new Dictionary<string, object>
            {
                { "AppName", app?.ApplicationName ?? string.Empty },
                { "Read", (app?.Scopes ?? string.Empty).Contains("read") ? "true" : "false" },
                { "Write", (app?.Scopes ?? string.Empty).Contains("write") ? "true" : "false" }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Claims = cliams,
                Expires = expiresAt,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public async static Task<bool> VerifyToken(string tokenString, string securityKey)
        {
            if (string.IsNullOrWhiteSpace(tokenString) || string.IsNullOrWhiteSpace(securityKey))
            {
                return false;
            }

            var keyBytes = Encoding.UTF8.GetBytes(securityKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var tokenHandler = new JsonWebTokenHandler();
            try
            {

                var result = await tokenHandler.ValidateTokenAsync(tokenString, tokenValidationParameters);
                return result.IsValid;
            }
            catch (SecurityTokenMalformedException ex)
            {
                return false;
            }
            catch (SecurityTokenExpiredException ex)
            {
                return false;
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
