using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GoogleServices
{
    public partial class LoginButton
    {
        public event Action<bool, TokenPair> AuthorizedCompleteEvent;

        private static readonly Color ButtonBackgrounDefaultColor = Color.FromArgb(255, 255, 69, 0);

        public LoginButton()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnTap(object sender, GestureEventArgs e)
        {
            if (String.IsNullOrEmpty(ClientId) || String.IsNullOrEmpty(SerectKey))
            {
                MessageBox.Show("Lack of Google Client ID or Serect Key!");
                return;
            }

            TokenPair tokenPair = await GoogleApis.AuthorizeGoogle(ClientId, SerectKey);

            #region Unused Code
            //TokenPair tokenPair = await authorization.Authorize("1076108784587-km9dnh2v3b3n5f5hi6p5mjstb7hqsn19.apps.googleusercontent.com", "ydQJtfJicoxfh5TF8vpHfcSj",
            //    new[] { GoogleScopes.UserinfoEmail });

            // Request a new access token using the refresh token (when the access token was expired)
            //TokenPair refreshTokenPair = await authorization.RefreshAccessToken(
            //    "km9dnh2v3b3n5f5hi6p5mjstb7hqsn19",
            //    "ydQJtfJicoxfh5TF8vpHfcSj",
            //    tokenPair.RefreshToken); 
            #endregion

            if (tokenPair != null && !String.IsNullOrEmpty(tokenPair.AccessToken))
            {
                if (AuthorizedCompleteEvent != null)
                {
                    AuthorizedCompleteEvent(true, tokenPair);
                }
            }
            else
            {
                if (AuthorizedCompleteEvent != null)
                {
                    AuthorizedCompleteEvent(false, null);
                }
            }
        }

        #region Google Client ID and Serect Key
        #region Client ID
        public static readonly DependencyProperty ClientIdProperty = DependencyProperty.Register("ClientId", typeof(string), typeof(LoginButton), new PropertyMetadata(null));

        public string ClientId
        {
            get { return (string)GetValue(ClientIdProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ClientIdProperty, value);
                });
            }
        } 
        #endregion

        #region Serect Key
        public static readonly DependencyProperty SerectKeyProperty = DependencyProperty.Register("SerectKey", typeof(string), typeof(LoginButton), new PropertyMetadata(null));

        public string SerectKey
        {
            get { return (string)GetValue(SerectKeyProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(SerectKeyProperty, value);
                });
            }
        }
        #endregion
        #endregion

        #region Button Background Color
        public static readonly DependencyProperty ButtonBackgroundColorProperty = DependencyProperty.Register("ButtonBackgroundColor", typeof(Brush), typeof(LoginButton),
            new PropertyMetadata(new SolidColorBrush(ButtonBackgrounDefaultColor), ButtonBackgroundColorChanged));

        private static void ButtonBackgroundColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newBackground = (SolidColorBrush)e.NewValue;

                if (newBackground != null)
                {
                    control.BackgroundBorder.Background = newBackground;
                }
            }
        }

        public Brush ButtonBackgroundColor
        {
            get { return (Brush)GetValue(ButtonBackgroundColorProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonBackgroundColorProperty, value);
                });
            }
        }
        #endregion

        #region Button Border
        #region Color
        public static readonly DependencyProperty ButtonBorderColorProperty = DependencyProperty.Register("ButtonBorderColor", typeof(Brush), typeof(LoginButton),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent), ButtonBorderColorChanged));

        private static void ButtonBorderColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newBorder = (SolidColorBrush)e.NewValue;

                if (newBorder != null)
                {
                    control.BackgroundBorder.BorderBrush = newBorder;
                }
            }
        }

        public Brush ButtonBorderColor
        {
            get { return (Brush)GetValue(ButtonBorderColorProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonBorderColorProperty, value);
                });
            }
        }
        #endregion

        #region Thickness
        public static readonly DependencyProperty ButtonBorderThicknessProperty = DependencyProperty.Register("ButtonBorderThickness", typeof(Thickness), typeof(LoginButton),
            new PropertyMetadata(new Thickness(), ButtonBorderStrokeChanged));

        private static void ButtonBorderStrokeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newBorderThickness = (Thickness)e.NewValue;

                control.BackgroundBorder.BorderThickness = newBorderThickness;
            }
        }

        public Thickness ButtonBorderThickness
        {
            get { return (Thickness)GetValue(ButtonBorderThicknessProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonBorderThicknessProperty, value);
                });
            }
        }
        #endregion

        #region Corner Radius
        public static readonly DependencyProperty ButtonBorderCornerRadiusProperty = DependencyProperty.Register("ButtonBorderCornerRadius", typeof(CornerRadius), typeof(LoginButton),
            new PropertyMetadata(new CornerRadius(), ButtonBorderCornerRadiusChanged));

        private static void ButtonBorderCornerRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newCornerRadius = (CornerRadius)e.NewValue;

                control.BackgroundBorder.CornerRadius = newCornerRadius;
            }
        }

        public CornerRadius ButtonBorderCornerRadius
        {
            get { return (CornerRadius)GetValue(ButtonBorderCornerRadiusProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonBorderCornerRadiusProperty, value);
                });
            }
        }
        #endregion
        #endregion

        #region Button Image Source
        public static readonly DependencyProperty ButtonImageSourceProperty = DependencyProperty.Register("ButtonImageSource", typeof(ImageSource), typeof(LoginButton), new PropertyMetadata(null, ButtonImageSourceChanged));

        private static void ButtonImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newImageSource = (ImageSource)e.NewValue;

                if (newImageSource != null)
                {
                    control.ButtonImage.Source = newImageSource;
                }
            }
        }

        public ImageSource ButtonImageSource
        {
            get { return (ImageSource)GetValue(ButtonImageSourceProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonImageSourceProperty, value);
                });
            }
        }
        #endregion

        #region Button Content
        #region Content
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent", typeof(string), typeof(LoginButton), new PropertyMetadata(String.Empty, new PropertyChangedCallback(ButtonContentChanged)));

        private static void ButtonContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newContent = (string)e.NewValue;

                if (!String.IsNullOrEmpty(newContent))
                {
                    control.ButtonContentTextBlock.Text = newContent;
                }
            }
        }

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ButtonContentProperty, value);
                });
            }
        }
        #endregion

        #region Font Size
        public static readonly DependencyProperty ContentFontSizeProperty = DependencyProperty.Register("ContentFontSize", typeof(int), typeof(LoginButton), new PropertyMetadata(24, ContentFontSizeChanged));

        private static void ContentFontSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var newFontSize = (int)e.NewValue;

                control.ButtonContentTextBlock.FontSize = newFontSize;
            }
        }

        public int ContentFontSize
        {
            get { return (int)GetValue(ContentFontSizeProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ContentFontSizeProperty, value);
                });
            }
        }
        #endregion

        #region Font Color
        public static readonly DependencyProperty ContentFontColorProperty = DependencyProperty.Register("ContentFontSize", typeof(Brush), typeof(LoginButton), new PropertyMetadata(new SolidColorBrush(Colors.White), ContentFontColorChanged));

        private static void ContentFontColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LoginButton control = sender as LoginButton;

            if (control != null)
            {
                var fontColor = (SolidColorBrush)e.NewValue;

                control.ButtonContentTextBlock.Foreground = fontColor;
            }
        }

        public Brush ContentFontColor
        {
            get { return (Brush)GetValue(ContentFontColorProperty); }
            set
            {
                Dispatcher.BeginInvoke(delegate
                {
                    SetValue(ContentFontColorProperty, value);
                });
            }
        }
        #endregion 
        #endregion
    }
}
