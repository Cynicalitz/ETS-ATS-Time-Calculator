using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ETS2_Time_Calculator
{
    public partial class rCalculator : UserControl
    {
        public rCalculator()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            for (int i = 0; i < 24; i++)
            {
                hourComboBox3.Items.Add(i);
            }

            for (int i = 0; i < 100; i++)
            {
                hourComboBox.Items.Add(i);
            }

            for (int i = 0; i < 60; i++)
            {
                minuteComboBox.Items.Add(i);;
                minuteComboBox3.Items.Add(i);
            }
            
        }

        private void OnConvertButtonClick(object sender, RoutedEventArgs e)
        {
            // Selected Real Time Left
            int realHours = (int)hourComboBox3.SelectedItem;
            int realMinutes = (int)minuteComboBox3.SelectedItem;
            TimeSpan realTimeLeft = new TimeSpan(realHours, realMinutes, 0);

            // Time left from your current position
            int inGameTimeLeftHours = (int)hourComboBox.SelectedItem;
            int inGameTimeLeftMinutes = (int)minuteComboBox.SelectedItem;
            TimeSpan inGameTimeLeft = new TimeSpan(inGameTimeLeftHours, inGameTimeLeftMinutes, 0);

            // Converted in-game time to real time
            TimeSpan convertedTimeLeft = ConvertToRealTime(inGameTimeLeft);

            // Calculate the difference
            TimeSpan roughlyTime = convertedTimeLeft - realTimeLeft;

            // Compare the TimeSpan difference
            if (roughlyTime > TimeSpan.Zero)
            {
                outputRTime.Text = $"You will not have enough time. You will have to do {FormatTime(roughlyTime)} more later.";
            }
            else
            {
                outputRTime.Text = $"You will have enough time.";
            }

            // Roughly max speed
            double assumedSpeedKmPerHour = 80;

            // Convert rougly time to in game time
            TimeSpan realRoughlyTimeInGame = ConvertToInGameTime(realTimeLeft);
            TimeSpan roughlyTimeInGame = ConvertToInGameTime(roughlyTime);

            // Calculate Distance
            double totalRealDistance = assumedSpeedKmPerHour * realRoughlyTimeInGame.TotalHours;
            double totalDistance = assumedSpeedKmPerHour * roughlyTimeInGame.TotalHours;

            // Check difference
            if (totalDistance < 0)
            {
                outputRSpeed.Text = $"You will travel roughly {Math.Round(totalRealDistance)}km";
            } else
            {
                outputRSpeed.Text = $"You will travel roughly {Math.Round(totalRealDistance)}km with your time left and {Math.Round(totalDistance)}km later.";
            }

        }

        private TimeSpan ConvertToInGameTime(TimeSpan realTime)
        {
            // Conversion ratio
            double realToInGameRatio = 20.0;

            // Convert real time to in-game time
            double inGameHours = realTime.TotalHours * realToInGameRatio;

            // Calculate remaining time in game
            int remainingHours = (int)inGameHours;
            int remainingMinutes = (int)((inGameHours - remainingHours) * 60);
            int remainingSeconds = (int)(((inGameHours - remainingHours) * 60 - remainingMinutes) * 60);

            return new TimeSpan(remainingHours, remainingMinutes, remainingSeconds);
        }

        private TimeSpan ConvertToRealTime(TimeSpan inGameTime)
        {
            // Conversion ratio
            double realToInGameRatio = 20.0;

            // Convert in-game time to real time
            double realHours = inGameTime.TotalHours / realToInGameRatio;

            // Calculate remaining time in real time
            int remainingHours = (int)realHours;
            int remainingMinutes = (int)((realHours - remainingHours) * 60);
            int remainingSeconds = (int)(((realHours - remainingHours) * 60 - remainingMinutes) * 60);

            return new TimeSpan(remainingHours, remainingMinutes, remainingSeconds);
        }

        private string FormatTime(TimeSpan timeSpan)
        {
            var components = new List<string>();

            void AddComponent(int value, string unit)
            {
                if (value > 0)
                    components.Add($"{value} {unit}{(value == 1 ? "" : "s")}");
            }

            AddComponent(timeSpan.Hours, "hour");
            AddComponent(timeSpan.Minutes, "minute");
            AddComponent(timeSpan.Seconds, "second");

            return string.Join(" ", components);
        }


    }
}
