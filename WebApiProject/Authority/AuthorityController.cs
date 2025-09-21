using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Authority
{
    [ApiController]
    [Route("auth")]
    public class AuthorityController : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId),
                    expires_at = DateTime.UtcNow.AddMinutes(10)
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

        private object CreateToken(string clientId)
        {
            return string.Empty;
        }
    }
}
