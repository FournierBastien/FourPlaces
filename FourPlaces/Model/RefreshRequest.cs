using Newtonsoft.Json;

namespace FourPlaces.Model
{
    public class RefreshRequest
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public RefreshRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}