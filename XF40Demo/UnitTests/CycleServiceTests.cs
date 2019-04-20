using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XF40Demo.Services;

namespace UnitTests
{
    [TestClass]
    public class CycleServiceTests
    {
        [TestMethod]
        public void CycleImminentTest()
        {
            int day = (int)(DateTime.UtcNow.DayOfWeek);
            int hour = DateTime.UtcNow.Hour;
            bool imminent = (day == 3 && hour >= 19) || (day == 4 && hour < 7);
            Assert.AreEqual(imminent, CycleService.CycleImminent());
        }

        [TestMethod]
        public void TimeTillTickTest()
        {
            TimeSpan time = CycleService.TimeTillTick();
            Assert.IsTrue(time != TimeSpan.MaxValue);
            Assert.IsTrue(time > TimeSpan.Zero);
            Assert.IsTrue(time.Days < 7);
        }
    }
}
