using Newtonsoft.Json;

namespace FourPlaces.Model
{
    public class LoginResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        public LoginResult(string accessToken, string refreshToken, int expiresIn, string tokenType)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            TokenType = tokenType;
        }
    }
}