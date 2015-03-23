using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace Flashlight.Models
{
    public class Torch : INotifyPropertyChanged, ICommand
    {
        private string imageSource;

        public string ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; NotifyPropertyChanged("ImageSource"); }
        }

        public Torch()
        {
            this.ImageSource = @"ms-appx:/Assets/Icons/power_off3.png";
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
    }
}
