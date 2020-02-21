using Microsoft.VisualStudio.TestTools.UnitTesting;
using XF40Demo.Models;

namespace UnitTests
{
    [TestClass]
    public class TemperatureScaleTests
    {
        [TestMethod]
        public void AverageTemperatureTest1()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Average = -200,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-328, data.Average);
        }

        [TestMethod]
        public void AverageTemperatureTest2()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Average = -173.25,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-279.85, data.Average);
        }

        [TestMethod]
        public void AverageTemperatureTest3()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Average = 80.28,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(176.504, data.Average);
        }

        [TestMethod]
        public void AverageTemperatureTest4()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Average = 158.4,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(317.12, data.Average);
        }

        [TestMethod]
        public void MinTemperatureTest1()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Min = -200,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-328, data.Min);
        }

        [TestMethod]
        public void MinTemperatureTest2()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Min = -173.25,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-279.85, data.Min);
        }

        [TestMethod]
        public void MinTemperatureTest3()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Min = 80.28,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(176.504, data.Min);
        }

        [TestMethod]
        public void MinTemperatureTest4()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Min = 158.4,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(317.12, data.Min);
        }

        [TestMethod]
        public void MaxTemperatureTest1()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Max = -200,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-328, data.Max);
        }

        [TestMethod]
        public void MaxTemperatureTest2()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Max = -173.25,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(-279.85, data.Max);
        }

        [TestMethod]
        public void MaxTemperatureTest3()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Max = 80.28,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(176.504, data.Max);
        }

        [TestMethod]
        public void MaxTemperatureTest4()
        {
            TemperatureSensorData data = new TemperatureSensorData
            {
                Max = 158.4,
                Scale = TemperatureScale.Fahrenheit
            };
            Assert.AreEqual(317.12, data.Max);
        }
    }
}
