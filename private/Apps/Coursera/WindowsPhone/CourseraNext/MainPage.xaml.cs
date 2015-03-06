using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Authentication.Web;
using CourseraNext.Managers;
using Windows.ApplicationModel.Activation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CourseraNext
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IWebAuthenticationContinuable
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();

            Current = this;

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        public void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            WebAuthenticationResult result = args.WebAuthenticationResult;


            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
            }
            else
            {
            }
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Uri StartUri = new Uri(String.Format("https://accounts.coursera.org/oauth2/v1/auth?response_type=code&client_id={0}&redirect_uri={1}&scope=view_profile&state={2}", "f91_rV2IsShrPS5fs9BVGw", System.Uri.EscapeUriString("https://www.facebook.com/pages/Pheonix-Labs/1444648675826026?"), "csrf_code1234"));
            System.Uri EndUri = new Uri("https://www.facebook.com/pages/Pheonix-Labs/1444648675826026?");

            WebAuthenticationBroker.AuthenticateAndContinue(StartUri, EndUri, null, WebAuthenticationOptions.None);
        }
    }
}
