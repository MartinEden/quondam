using NUnit.Framework;
using System;
using System.Threading;

namespace MartinEden.Quondam.Tests
{
    public class ClockTests
    {
        [Test]
        public void RealClockAdvancesInRealTime()
        {
            IClock clock = new RealClock();
            var now = clock.Now;
            Thread.Sleep(10);
            Assert.Greater(clock.Now, now);
        }

        [Test]
        public void FakeClockDoesNotAdvanceByItself()
        {
            var clock = new FakeClock(DateTime.Now);
            var now = clock.Now;
            Thread.Sleep(10);
            Assert.AreEqual(clock.Now, now);
        }

        [Test]
        public void FakeClockAdvancesWhenToldTo()
        {
            var clock = new FakeClock(DateTime.Now);
            var elapsed = TimeSpan.FromMinutes(1);
            var expected = clock.Now + elapsed;
            clock.Advance(elapsed);
            Assert.AreEqual(expected, clock.Now);
        }
    }
}
