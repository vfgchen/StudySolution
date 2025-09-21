using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Authority;

namespace WebApiProject.Filters.AuthFilters
{
    public class JwtTokenAuthrizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 1. Get Authorization header from the request
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 2. Get rid of the Bearer prefix
            var tokenString = authorizationHeader.ToString();
            if (!tokenString.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            tokenString = tokenString.Substring("Bearer ".Length).Trim();

            // 3. Get SecretKey from Configuration
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var securityKey = configuration?["SecurityKey"] ?? string.Empty;

            // 4. Verify the token
            if (!await Authenticator.VerifyToken(tokenString, securityKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
