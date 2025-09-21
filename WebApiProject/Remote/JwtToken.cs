using System.Text.Json.Serialization;

namespace WebApiProject.Remote
{
    public class JwtToken
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_at")]
        public DateTime? ExpriresAt { get; set; }
    }
}
