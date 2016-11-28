using System;

namespace MartinEden.Quondam
{
    internal class RealClock : IClock
    {
        DateTime IClock.Now
        {
            get { return DateTime.Now; }
        }
    }
}
