using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace GoogleServices
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            string authEndpoint = parameters["authEndpoint"];
            string clientId = parameters["clientId"];
            string scope = parameters["scope"];

            string uri = string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&scope={3}",
                authEndpoint,
                clientId,
                "urn:ietf:wg:oauth:2.0:oob",
                scope);

            LoginBrowser.Navigate(new Uri(uri, UriKind.Absolute));
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            string title = (string)LoginBrowser.InvokeScript("eval", "document.title.toString()");

            if (title.StartsWith("Success"))
            {
                string authorizationCode = title.Substring(title.IndexOf('=') + 1);

                PhoneApplicationService.Current.State["OAuth_Demo.AuthorizationCode"] = authorizationCode;

                NavigationService.GoBack();
            }
        }
    }
}