using System.Net.Http.Headers;
using WebApiProject.Authority;

namespace WebApiProject.Remote
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "PersonApi";
        private const string AuthApiName = "AuthorityApi";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public WebApiExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.GetAsync(relativeUrl);
            await HandlePotentialException(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
            await HandlePotentialException(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            await HandlePotentialException(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.DeleteAsync(relativeUrl);
            await HandlePotentialException(response);
        }

        private async Task HandlePotentialException(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }

        private async Task AddJwtToHeader(HttpClient httpClient)
        {
            // Authencate
            var authApiClient = httpClientFactory.CreateClient(AuthApiName);
            var app = AppRepository.GetDefaultApplication();
            var response = await authApiClient.PostAsJsonAsync("auth", new AppCredential
            {
                // Normally, read configuration from appsettting.json
                // ClientId = configuration["ClientId"] ?? string.Empty,
                // Secret = configuration["Secret"] ?? string.Empty

                // Using AppRepository get app information
                ClientId = app?.ClientId ?? string.Empty,
                Secret = app?.Secret ?? string.Empty,
            });
            response.EnsureSuccessStatusCode();

            // Get the JWT
            var jwtToken = await response.Content.ReadFromJsonAsync<JwtToken>();

            // Pass the JWT to endpoints through the http header
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken?.AccessToken);
        }

    }
}
