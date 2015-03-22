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
                var battery = Battery.GetDefault();
                this.InitializeComponent();

                this.NavigationCacheMode = NavigationCacheMode.Required;

                this.navigationHelper = new NavigationHelper(this);
                this.navigationHelper.LoadState += navigationHelper_LoadState;
                this.navigationHelper.SaveState += navigationHelper_SaveState;

                Application.Current.Resuming += Current_Resuming;
                //this.Loaded += MainPage_Loaded;

                battery.RemainingChargePercentChanged += Battery_RemainingChargePercentChanged;

                UpdateBatteryWdiget();

                // check if the torch intensity changer is enabled
                var torch = captureManager.VideoDeviceController.TorchControl;

                if(torch.PowerSupported)
                {

                }

            }
            catch(Exception)
            {

            }
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // no implementation yet
        }

        void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.defaultViewModel["Battery"] = new Models.Battery();
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
            this.navigationHelper.OnNavigatedTo(e);
        }

        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desiredCamera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredCamera);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desiredCamera));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            //base.OnNavigatedFrom(e);
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

                    //var videoEncodingProperties = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Vga);

                    //// Start Video Recording
                    //var videoStorageFile = await KnownFolders.VideosLibrary.CreateFileAsync("tempVideo.mp4", CreationCollisionOption.GenerateUniqueName);
                    //await captureManager.StartRecordToStorageFileAsync(videoEncodingProperties, videoStorageFile);
                    

                    //torch.PowerPercent = 80.0f;
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

        private void LightIntensitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }
    }
}
