using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace GoogleServices
{
    public class OAuthAuthorization
    {
        private readonly string authEndpoint;
        private readonly string tokenEndpoint;
        private readonly PhoneApplicationFrame frame;

        public OAuthAuthorization(string authEndpoint, string tokenEndpoint)
        {
            this.authEndpoint = authEndpoint;
            this.tokenEndpoint = tokenEndpoint;
            frame = (PhoneApplicationFrame)(Application.Current.RootVisual);
        }

        public async Task<TokenPair> Authorize(string clientId, string clientSecret, IEnumerable<string> scopes)
        {
            string uri = string.Format("/GoogleServices;component/LoginPage.xaml?authEndpoint={0}&clientId={1}&scope={2}",
                                       authEndpoint, clientId, string.Join(" ", scopes));

            SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

            Observable.FromEvent<NavigatingCancelEventHandler, NavigatingCancelEventArgs>(
                h => new NavigatingCancelEventHandler(h),
                h => frame.Navigating += h,
                h => frame.Navigating -= h)
                      .SkipWhile(h => h.EventArgs.NavigationMode != NavigationMode.Back)
                      .Take(1)
                      .Subscribe(delegate
                      {
                          semaphore.Release();
                          semaphore.Dispose();
                      });

            frame.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));

            await semaphore.WaitAsync();

            if (PhoneApplicationService.Current.State.ContainsKey("OAuth_Demo.AuthorizationCode"))
            {
                string authorizationCode = (string)PhoneApplicationService.Current.State["OAuth_Demo.AuthorizationCode"];

                return await RequestAccessToken(authorizationCode, clientId, clientSecret);
            }
            return null;
        }

        public async Task<TokenPair> RefreshAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(tokenEndpoint);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await httpRequest.GetRequestStreamAsync())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write("refresh_token=" + Uri.EscapeDataString(refreshToken) + "&");
                    writer.Write("client_id=" + Uri.EscapeDataString(clientId) + "&");
                    writer.Write("client_secret=" + Uri.EscapeDataString(clientSecret) + "&");
                    writer.Write("grant_type=refresh_token");
                }
            }

            using (WebResponse response = await httpRequest.GetResponseAsync())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    TokenPair tokenPair = JsonConvert.DeserializeObject<TokenPair>(result);
                    tokenPair.RefreshToken = refreshToken;

                    return tokenPair;
                }
            }
        }

        private async Task<TokenPair> RequestAccessToken(string authorizationCode, string clientId, string clientSecret)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(tokenEndpoint);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await httpRequest.GetRequestStreamAsync())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write("code=" + Uri.EscapeDataString(authorizationCode) + "&");
                    writer.Write("client_id=" + Uri.EscapeDataString(clientId) + "&");
                    writer.Write("client_secret=" + Uri.EscapeDataString(clientSecret) + "&");
                    writer.Write("redirect_uri=" + Uri.EscapeDataString("urn:ietf:wg:oauth:2.0:oob") + "&");
                    writer.Write("grant_type=authorization_code");
                }
            }

            using (WebResponse response = await httpRequest.GetResponseAsync())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<TokenPair>(result);
                }
            }
        }
    }
}
