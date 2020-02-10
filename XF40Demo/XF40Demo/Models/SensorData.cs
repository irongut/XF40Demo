using Newtonsoft.Json;

namespace XF40Demo.Models
{
    public class SensorData
    {
        [JsonProperty(PropertyName = "av")]
        public double Average { get; internal set; }

        [JsonProperty(PropertyName = "ct")]
        public uint Count { get; internal set; }

        [JsonProperty(PropertyName = "mn")]
        public double Min { get; internal set; }

        [JsonProperty(PropertyName = "mx")]
        public double Max { get; internal set; }

        public SensorData(double avg, uint count, double min, double max)
        {
            Average = avg;
            Count = count;
            Min = min;
            Max = max;
        }
    }
}
