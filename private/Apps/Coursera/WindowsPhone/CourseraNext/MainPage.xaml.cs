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
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft;
using Newtonsoft.Json;
using CourseraNext.Models;

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

        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            WebAuthenticationResult result = args.WebAuthenticationResult;

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                // fetch code from url
                string authCode = GetErrorCodeFromString(result.ResponseData);

                if (authCode != null)
                {
                    var accessToken = await GetAccessTokenFromCode(authCode);

                    string profileInfo = await GetRequest("https://api.coursera.org/api/externalBasicProfiles.v1?q=me&fields=name,profilephoto,timezone,locale,privacy", accessToken);

                    string enrollments = await GetRequest("https://api.coursera.org/api/users/v1/me/enrollments", accessToken);

                    var objectTuple = GetObjectsFomJson(enrollments, profileInfo);
                }
                else
                {
                    // todo throw error
                }
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {

            }
            else
            {

            }
        }

        public Tuple<ProfileInfo, List<Course>, List<Enrollment>>GetObjectsFomJson(string enrollmentsJson, string profileJson)
        {
            ProfileInfo profileInfo = new ProfileInfo();
            List<Enrollment> enrollments = new List<Enrollment>();
            List<Course> courses = new List<Course>();

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(enrollmentsJson);

            enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(JsonConvert.SerializeObject(dict["enrollments"]));

            courses = JsonConvert.DeserializeObject<List<Course>>(JsonConvert.SerializeObject(dict["courses"]));

            dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(profileJson);

            profileInfo = JsonConvert.DeserializeObject<List<ProfileInfo>>(JsonConvert.SerializeObject(dict["elements"]))[0];

            return new Tuple<ProfileInfo, List<Course>, List<Enrollment>>(profileInfo, courses, enrollments);
        }
      

        private string GetErrorCodeFromString(string redirectUrl)
        {
            string[] tokens = redirectUrl.Split(new char[] { '&' });

            foreach (var token in tokens)
            {
                if (token.Contains("code="))
                {
                    return token.Replace("code=", string.Empty);
                }
            }

            return null;
        }

        private async Task<string> GetAccessTokenFromCode(string code)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("code", code));
            values.Add(new KeyValuePair<string, string>("client_secret", Constants.App.ClientSecret));
            values.Add(new KeyValuePair<string, string>("client_id", Constants.App.ClientId));
            values.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            values.Add(new KeyValuePair<string, string>("redirect_uri", Constants.App.RedirectUri));

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("https://accounts.coursera.org/oauth2/v1/token", new FormUrlEncodedContent(values));
            var responseString = await response.Content.ReadAsStringAsync();

            var responseDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            if (responseDict.ContainsKey("access_token"))
            {
                return responseDict["access_token"];
            }

            return null;
        }

        private async Task<string> GetRequest(string url, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await client.GetStringAsync(new Uri(url));
            return response;
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
