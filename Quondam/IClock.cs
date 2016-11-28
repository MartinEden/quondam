using System;

namespace MartinEden.Quondam
{
    /// <summary>
    /// A clock provides a way of getting the current time. The purpose here is to take the
    /// global static DateTime object, and wrap it in an interface that allows us to mock
    /// it in the tests. That way, the tests can control timing precisely.
    /// 
    /// Note that this in an internal interface, and the PasswordManager constructor that
    /// takes a clock is also internal. This is so that this detail is hidden from consumers
    /// of the library - they just get the expected behaviour with a real clock. Because the
    /// unit tests have access to internal members of this library, they can still make use
    /// of this interface and the constructor.
    /// </summary>
    internal interface IClock
    {
        DateTime Now { get; }
    }
}
