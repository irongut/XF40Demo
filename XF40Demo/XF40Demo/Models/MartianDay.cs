using System;

namespace XF40Demo.Models
{
    public class MartianDay
    {
        public uint Sol { get; }

        public SensorData AtmosphericTemp { get; }

        public SensorData HorizontalWindSpeed { get; }

        public SensorData AtmosphericPressure { get; }

        public WindDirectionSensorData WindDirection { get; }

        public string Season { get; }

        public DateTime FirstUTC { get; }

        public DateTime LastUTC { get; }

        public MartianDay(uint sol, SensorData temp, SensorData speed, SensorData pressure, WindDirectionSensorData direction, string season, DateTime first, DateTime last)
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
    }
}
