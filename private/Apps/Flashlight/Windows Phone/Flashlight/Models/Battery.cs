using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashlight.Models
{
    public class Battery: INotifyPropertyChanged
    {
        private string remainingPercentage;

        public string RemainingPercentage
        {
            get { return string.Concat(remainingPercentage, "%"); }
            set 
            { 
                remainingPercentage = value;
                NotifyPropertyChanged("RemainingPercentage");
            }
        }

        private string timeLeft;

        public string TimeLeft
        {
            get { return timeLeft; }
            set 
            { 
                timeLeft = value;
                NotifyPropertyChanged("TimeLeft");
            }
        }

        private string textColor;

        public string TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                NotifyPropertyChanged("TextColor");
            }
        }
              

        public Battery()
        {
            var battery = Windows.Phone.Devices.Power.Battery.GetDefault();

            RemainingPercentage = battery.RemainingChargePercent.ToString();

            // TODO: Compute the remaining batter time based on factors like wifi, brightness
            //TimeLeft = GetFormattedTime(battery.RemainingDischargeTime);

            TextColor = GetTextColor(battery.RemainingChargePercent);

            battery.RemainingChargePercentChanged += battery_RemainingChargePercentChanged;
        }

        void battery_RemainingChargePercentChanged(object sender, object e)
        {
            var battery = Windows.Phone.Devices.Power.Battery.GetDefault();

            RemainingPercentage = battery.RemainingChargePercent.ToString();

            // TODO: Compute the remaining batter time based on factors like wifi, brightness
            //TimeLeft = battery.TimeLeft;

            TextColor = GetTextColor(battery.RemainingChargePercent);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string GetFormattedTime(TimeSpan timeleft)
        {
            int days = timeleft.Days;
            int hours = timeleft.Hours;
            int minutes = timeleft.Minutes;

            string formattedTime = "";
            if(days != 0)
            {
                formattedTime += string.Concat(days.ToString(), " Days, ");
            }

            if(hours != 0)
            {
                formattedTime += string.Concat(hours.ToString(), " Hours, ");
            }

            if(minutes != 0)
            {
                formattedTime += string.Concat(minutes.ToString(), " Minutes");
            }

            return formattedTime;
        }

        public string GetTextColor(int remainingTime)
        {
            if(remainingTime >= 70)
            {
                return "LightGreen";
            }
            else if(remainingTime >= 40)
            {
                return "Orange";
            }
            else
            {
                return "Red";
            }
        }
    }
}
