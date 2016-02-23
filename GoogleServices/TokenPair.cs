using Newtonsoft.Json;

namespace GoogleServices
{
    public class TokenPair
    {
        public TokenPair(string accessToken, string tokenType, int expiresIn, string refreshToken)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
