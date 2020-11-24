using System;
using System.ComponentModel;

namespace XF40Demo.Models
{
    public class MartianDay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public uint Sol { get; }

        public TemperatureSensorData AtmosphericTemp { get; }

        public SensorData HorizontalWindSpeed { get; }

        public SensorData AtmosphericPressure { get; }

        public WindDirectionSensorData WindDirection { get; }

        public string Season { get; }

        public DateTime FirstUTC { get; }

        public DateTime LastUTC { get; }

        public MartianDay(uint sol, TemperatureSensorData temp, SensorData speed, SensorData pressure, WindDirectionSensorData direction, string season, DateTime first, DateTime last)
        {
            Sol = sol;
            AtmosphericTemp = temp;
            HorizontalWindSpeed = speed;
            AtmosphericPressure = pressure;
            WindDirection = direction;
            Season = season;
            FirstUTC = first;
            LastUTC = last;
        }

        public void SetTemperatureScale(TemperatureScale scale)
        {
            if (AtmosphericTemp != null)
            {
                AtmosphericTemp.Scale = scale;
                OnPropertyChanged(nameof(AtmosphericTemp));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
