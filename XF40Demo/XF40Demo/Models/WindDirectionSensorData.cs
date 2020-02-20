using System.Collections.Generic;

namespace XF40Demo.Models
{
    public class WindDirectionSensorData
    {
        public List<CompassPoint> CompassPoints { get; }

        public CompassPoint MostCommon { get; internal set; }

        public WindDirectionSensorData()
        {
            CompassPoints = new List<CompassPoint>();
        }

        public WindDirectionSensorData(List<CompassPoint> points, CompassPoint common)
        {
            CompassPoints = points;
            MostCommon = common;
        }
    }
}
