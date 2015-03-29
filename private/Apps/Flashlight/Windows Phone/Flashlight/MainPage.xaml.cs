using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Devices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Phone.Devices.Power;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Flashlight.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Flashlight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get
            {
                return this.defaultViewModel;
            }
        }

        private readonly NavigationHelper navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }


        public static MediaCapture captureManager;

        public static bool isCaptureMgrDisposed;

        public static float userTorchPercent;

        public MainPage()
        {
            try
            {
                //var dialog = new MessageDialog("Constructor called");
                //dialog.ShowAsync();
                this.InitializeComponent();

                this.NavigationCacheMode = NavigationCacheMode.Required;

                this.navigationHelper = new NavigationHelper(this);
                this.navigationHelper.LoadState += navigationHelper_LoadState;
                this.navigationHelper.SaveState += navigationHelper_SaveState;

                Application.Current.Resuming += Current_Resuming;

            }
            catch (Exception)
            {

            }
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            DisposeViewModel();
        }

        public async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await InitialiseViewModel();
        }

        public async Task InitialiseViewModel()
        {
            this.DefaultViewModel["Battery"] = new Models.Battery();

            var torch = new Models.Torch();

            this.DefaultViewModel["Torch"] = torch;

            await torch.InitialiseCaptureManager();
        }

        public void DisposeViewModel()
        {
            // no implementation yet
            var torch = this.DefaultViewModel["Torch"] as Models.Torch;

            torch.DisposeCaptureManager();
        }

        private async void Current_Resuming(object sender, object e)
        {
            try
            {
                DisposeViewModel();
                await InitialiseViewModel();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message + ex.StackTrace);
                dialog.ShowAsync();
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
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

            //var dialog = new MessageDialog("Navigated called");
            //await dialog.ShowAsync();
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void PowerImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //TODO: Move this to commands
            var torch = this.DefaultViewModel["Torch"] as Models.Torch;
            torch.PowerButtonTapped();
        }

        private void LightIntensitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void SOSSwitch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;

            var torch = this.DefaultViewModel["Torch"] as Models.Torch;

            if (toggleButton.IsChecked == true)
            {
                torch.SOSTurnedOn((int)StrobeSlider.Value);
            }
            else
            {
                torch.SOSTurnedOff();
            }
        }

        private void StrobeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.DefaultViewModel.ContainsKey("Torch"))
            {
                var torch = this.DefaultViewModel["Torch"] as Models.Torch;
                torch.SOSTurnedOff();
                torch.SOSTurnedOn((int)StrobeSlider.Value);
            }
        }
    }

}