using Newtonsoft.Json;

namespace XF40Demo.Models
{
    public class SensorData
    {
        [JsonProperty(PropertyName = "av")]
        public virtual double Average { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public uint Count { get; set; }

        [JsonProperty(PropertyName = "mn")]
        public virtual double Min { get; set; }

        [JsonProperty(PropertyName = "mx")]
        public virtual double Max { get; set; }
    }
}
