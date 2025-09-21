namespace WebApiProject.Remote
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "PersonApi";
        private readonly IHttpClientFactory httpClientFactory;

        public WebApiExecutor(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.GetAsync(relativeUrl);
            await HandlePotentialException(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
            await HandlePotentialException(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            await HandlePotentialException(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
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

    }
}
