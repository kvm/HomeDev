using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Flashlight.Models
{
    public class Torch : INotifyPropertyChanged, ICommand
    {
        private string imageSource;

        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        private bool isSOSTurnedOn;

        public bool IsSOSTurnedOn
        {
            get { return isSOSTurnedOn; }
            set { isSOSTurnedOn = value; NotifyPropertyChanged("IsSOSTurnedOn"); }
        }

        private DispatcherTimer dispatcherTimer;

        private int SOSOnTimeInMilliseconds;

        private int SOSOffTimeInMilliseconds;

        public Torch()
        {
            this.ImageSource = @"ms-appx:/Assets/Icons/power_off3.png";
            this.IsSOSTurnedOn = false;
            this.dispatcherTimer = new DispatcherTimer();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MediaCapture captureManager;
        
        public async Task InitialiseCaptureManager()
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

        private async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desiredCamera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredCamera);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desiredCamera));
        }

        public void DisposeCaptureManager()
        {
            this.captureManager.Dispose();
        }

        public void PowerButtonTapped()
        {
            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                if (torch.Enabled)
                {
                    TurnOffPowerButtonImage();
                    torch.Enabled = false;
                    SOSTurnedOff();
                }
                else
                {
                    TurnOnPowerButtonImage();
                    torch.Enabled = true;
                    SOSTurnedOff();
                }
            }
        }

        private void TurnOffPowerButtonImage()
        {
            this.ImageSource = @"ms-appx:/Assets/Icons/power_off3.png";
        }

        private void TurnOnPowerButtonImage()
        {
            this.ImageSource = @"ms-appx:/Assets/Icons/power_on3.png";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            PowerButtonTapped();
        }

        public void SOSTurnedOn(int sliderValue)
        {
            this.IsSOSTurnedOn = true;
            sliderValue = 100 - sliderValue;

            this.SOSOnTimeInMilliseconds = ((sliderValue / 35) + 1) * 500 + 250;
            this.SOSOffTimeInMilliseconds = ((sliderValue / 35) + 1) * 500 + 250;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler<object>(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, this.SOSOnTimeInMilliseconds);
            dispatcherTimer.Start();

            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                torch.Enabled = true;
                TurnOnPowerButtonImage();
            }
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            if(this.SOSOffTimeInMilliseconds >= 750)
            {
                var torch = captureManager.VideoDeviceController.TorchControl;
                if (torch.Supported)
                {
                    if(torch.Enabled)
                    {
                        torch.Enabled = false;
                    }
                    else
                    {
                        torch.Enabled = true;
                    }
                }
            }
        }

        public void SOSTurnedOff()
        {
            dispatcherTimer.Stop();
            
            this.IsSOSTurnedOn = false;

            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                torch.Enabled = false;
                TurnOffPowerButtonImage();
            }
        }

        public void TurnOnTorch()
        {
            TurnOnPowerButtonImage();
            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                torch.Enabled = true;
            }
        }

        public void TurnOffTorch()
        {
            TurnOffPowerButtonImage();
            var torch = captureManager.VideoDeviceController.TorchControl;
            if (torch.Supported)
            {
                torch.Enabled = false;
            }

            if (this.IsSOSTurnedOn)
            {
                this.SOSTurnedOff();
            }
        }
    }
}
