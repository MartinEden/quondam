using System;

namespace MartinEden.Quondam.Tests
{
    public class FakeClock : IClock
    {
        private DateTime now;

        public FakeClock(DateTime initialTime)
        {
            now = initialTime;
        }

        public void Advance(TimeSpan elapsedTime)
        {
            now += elapsedTime;
        }

        public DateTime Now
        {
            get { return now; }
        }
    }
}
