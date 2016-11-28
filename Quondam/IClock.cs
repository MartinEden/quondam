using System;

namespace MartinEden.Quondam
{
    internal interface IClock
    {
        DateTime Now { get; }
    }
}
