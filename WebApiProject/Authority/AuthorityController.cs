using Microsoft.AspNetCore.Mvc;

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
            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                var securityKey = configuration["SecurityKey"] ?? string.Empty;
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, securityKey),
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
        
    }
}
