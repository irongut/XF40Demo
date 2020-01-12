using System.Collections.Generic;

namespace XF40Demo.Models
{
    public class WindDirectionSensorData
    {
        public List<CompassPoint> CompassPoints { get; }

        public CompassPoint MostCommon { get; }

        public WindDirectionSensorData(List<CompassPoint> points, CompassPoint common)
        {
            CompassPoints = points;
            MostCommon = common;
        }
    }
}
