using Newtonsoft.Json;

namespace XF40Demo.Models
{
    public class CompassPoint
    {
        [JsonProperty(PropertyName = "compass_degrees")]
        public float CompassDegrees { get; internal set; }

        [JsonProperty(PropertyName = "compass_point")]
        public string CompassPointName { get; internal set; }

        [JsonProperty(PropertyName = "compass_right")]
        public double CompassRight { get; internal set; }

        [JsonProperty(PropertyName = "compass_up")]
        public double CompassUp { get; internal set; }

        [JsonProperty(PropertyName = "ct")]
        public uint Count { get; internal set; }

        public CompassPoint(float degrees, string name, double right, double up, uint count)
        {
            CompassDegrees = degrees;
            CompassPointName = name;
            CompassRight = right;
            CompassUp = up;
            Count = count;
        }
    }
}
