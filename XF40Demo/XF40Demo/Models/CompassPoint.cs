namespace XF40Demo.Models
{
    public class CompassPoint
    {
        public float CompassDegrees { get; }

        public string CompassPointName { get; }

        public double CompassRight { get; }

        public double CompassUp { get; }

        public uint Count { get; }

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
