using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Flashlight.UserControls
{
    public class CompassControl : Control
    {
        private Windows.Devices.Sensors.Compass compass;

        public CompassControl()
        {
            this.DefaultStyleKey = typeof(CompassControl);

            this.Loaded += CompassControl_Loaded;
            this.Unloaded += CompassControl_Unloaded;
        }

        public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register("Heading",
            typeof(double), typeof(CompassControl), new PropertyMetadata(0.0));

        public double Heading
        {
            get
            {
                return (double)GetValue(HeadingProperty);
            }
            set
            {
                SetValue(HeadingProperty, value);
            }
        }

        private void CompassControl_Loaded(object sender, RoutedEventArgs e)
        {
            compass = Windows.Devices.Sensors.Compass.GetDefault();
            if (compass != null)
            {
                compass.ReadingChanged += CompassReadingChanged;
            }
        }

        private void CompassControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (compass != null)
            {
                compass.ReadingChanged -= CompassReadingChanged;
            }
        }

        private async void CompassReadingChanged(Windows.Devices.Sensors.Compass sender, Windows.Devices.Sensors.CompassReadingChangedEventArgs args)
        {
            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                {
                    Heading = args.Reading.HeadingMagneticNorth;
                }));
            }
            catch { }
        }
    }
}
