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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Flashlight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MediaCapture captureManager;

        public static bool isCaptureMgrDisposed;

        public MainPage()
        {
            try
            {
                //var dialog = new MessageDialog("Constructor called");
                //dialog.ShowAsync();
                var battery = Battery.GetDefault();
                this.InitializeComponent();

                this.NavigationCacheMode = NavigationCacheMode.Required;

                Application.Current.Resuming += Current_Resuming;
                //this.Loaded += MainPage_Loaded;

                battery.RemainingChargePercentChanged += Battery_RemainingChargePercentChanged;

                UpdateBatteryWdiget();
            }
            catch(Exception)
            {

            }
        }

        private void Battery_RemainingChargePercentChanged(object sender, object e)
        {
            UpdateBatteryWdiget();
        }

        private void Current_Resuming(object sender, object e)
        {
            TurnOffPowerButtonImage();
            UpdateBatteryWdiget();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var cameraID = await GetCameraID(Windows.Devices.Enumeration.Panel.Back);
            captureManager = new MediaCapture();

            await captureManager.InitializeAsync(new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
                AudioDeviceId = string.Empty,
                VideoDeviceId = cameraID.Id
            });
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
        }

        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desiredCamera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredCamera);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desiredCamera));
        }

        private async void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Windows.Media.Capture.MediaCapture mediaCatpure = new MediaCapture();
            //await mediaCatpure.InitializeAsync(new MediaCaptureInitializationSettings {
            //});
            ////mediaCatpure.VideoDeviceController.FlashControl. = true;
            //mediaCatpure.VideoDeviceController.TorchControl.Enabled = true;

            if (isCaptureMgrDisposed)
            {
                await Flashlight.App.InitialiseMediaCapture();
                isCaptureMgrDisposed = false;
            }

            // then to turn on/off camera
            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                if(torch.Enabled)
                {
                    PowerImageButton.Source = new BitmapImage(new Uri(@"/Assets/Icons/power_off1.jpg", UriKind.Relative));
                    torch.Enabled = false;
                }
                else
                {
                    PowerImageButton.Source = new BitmapImage(new Uri(@"/Assets/Icons/power_on1.jpg", UriKind.Relative));
                    torch.Enabled = true;
                }
            }

            //captureManager.Dispose();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            captureManager.Dispose();
        }

        private async void PowerImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isCaptureMgrDisposed)
            {
                await Flashlight.App.InitialiseMediaCapture();
                isCaptureMgrDisposed = false;
            }

            // then to turn on/off camera
            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                if (torch.Enabled)
                {
                    TurnOffPowerButtonImage();
                    torch.Enabled = false;
                }
                else
                {
                    TurnOnPowerButtonImage();
                    torch.Enabled = true;
                }
            }
        }

        private void TurnOnPowerButtonImage()
        {
            PowerImageButton.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Icons/power_on3.png", UriKind.Absolute));
        }

        private void TurnOffPowerButtonImage()
        {
            PowerImageButton.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Icons/power_off3.png", UriKind.Absolute));
        }

        //private async void SosImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    if (isCaptureMgrDisposed)
        //    {
        //        await Flashlight.App.InitialiseMediaCapture();
        //        isCaptureMgrDisposed = false;
        //    }

        //    SosImageButton.Opacity = 1.0;
        //}

        private void UpdateBatteryWdiget()
        {
            var battery = Battery.GetDefault();

            BatteryPercentageText.Text = string.Format("{0}%", battery.RemainingChargePercent.ToString());

            BatteryTimeText.Text = string.Format("{0]hrs {1}mins", battery.RemainingDischargeTime.TotalHours == 0.0 ? string.Empty : battery.RemainingDischargeTime.Hours.ToString(), battery.RemainingDischargeTime.Minutes.ToString());
        }
    }
}
