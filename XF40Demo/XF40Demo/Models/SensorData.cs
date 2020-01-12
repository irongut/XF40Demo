namespace XF40Demo.Models
{
    public class SensorData
    {
        public double Average { get; }

        public uint Count { get; }

        public double Min { get; }

        public double Max { get; }

        public SensorData(double avg, uint count, double min, double max)
        {
            Average = avg;
            Count = count;
            Min = min;
            Max = max;
        }
    }
}
