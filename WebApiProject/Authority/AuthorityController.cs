using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApiProject.Authority
{
    [ApiController]
    [Route("auth")]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthorityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        [HttpPost]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId, expiresAt),
                    expires_at = expiresAt
                });
            }
            else
            {
                this.ModelState.AddModelError("Unauthorized", "You are not authorized");
                var problemDetails = new ValidationProblemDetails(this.ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
        }

        private string CreateToken(string clientId, DateTime expiresAt)
        {
            // Signing Key
            var SecurityKey = configuration["SecurityKey"] ?? string.Empty;

            // Algorithm
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)),
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
    }
}
