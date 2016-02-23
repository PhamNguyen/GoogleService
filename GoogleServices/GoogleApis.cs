using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleServices
{
    public static class GoogleApis
    {
        public static async Task<TokenPair> AuthorizeGoogle(string clientId, string serectKey)
        {
            OAuthAuthorization authorization = new OAuthAuthorization("https://accounts.google.com/o/oauth2/auth", "https://accounts.google.com/o/oauth2/token");

            return await authorization.Authorize(clientId, serectKey, new[] { GoogleScopes.UserinfoEmail });
        }

        public static async Task<Profile> LoadUserProfile(string accessToken)
        {
            var httpClient = new HttpClient();

            var url = String.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}", accessToken);

            string profileResultString = await httpClient.GetStringAsync(url);

            if (!String.IsNullOrEmpty(profileResultString))
            {
                var userProfile = JsonConvert.DeserializeObject<Profile>(profileResultString);
                return userProfile;
            }
            return null;
        }
    }
}
