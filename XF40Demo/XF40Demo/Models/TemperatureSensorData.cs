using Newtonsoft.Json;

namespace XF40Demo.Models
{
    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit
    }

    public class TemperatureSensorData : SensorData
    {
        private double _average;

        [JsonProperty(PropertyName = "av")]
        public override double Average
        {
            get
            {
                return Scale == TemperatureScale.Fahrenheit ? (_average * 1.8) + 32 : _average;
            }
            set
            {
                if (_average != value)
                {
                    _average = value;
                }
            }
        }

        public double _min;

        [JsonProperty(PropertyName = "mn")]
        public override double Min
        {
            get
            {
                return Scale == TemperatureScale.Fahrenheit ? (_min * 1.8) + 32 : _min;
            }
            set
            {
                if (_min != value)
                {
                    _min = value;
                }
            }
        }

        public double _max;

        [JsonProperty(PropertyName = "mx")]
        public override double Max
        {
            get
            {
                return Scale == TemperatureScale.Fahrenheit ? (_max * 1.8) + 32 : _max;
            }
            set
            {
                if (_max != value)
                {
                    _max = value;
                }
            }
        }

        public TemperatureScale Scale { get; set; }
    }
}
