using System.Text.Json;

namespace WebApiProject.Remote
{
    public class WebApiException : Exception
    {
        public ErrorResponse? ErrorResponse { get; }

        public WebApiException(string errorJson)
        {
            this.ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
        }
    }
}
