# Quondam
A Latin word meaning "one-time" (if you squint a bit).

This library is a one-time password generation & validation library I wrote for a job application. The spec was to have a C# library or program capable of:

* Generating a one-time password for any given user name.
* Validating the password by sending a user name and password.
* Expiring passwords after 30 seconds if they aren't used

This solution uses NUnit for testing and NLog for creating an audit trail of failed login attempts. I salt and hash passwords before storing them. There is no persistence layer for the passwords - everything is in memory.

## Testing with timings
I've used a pattern I find myself repeatedly recreating for doing unit tests involving timing. It looks like this:

```c#
internal interface IClock
{
    DateTime Now { get; }
}

public PasswordManager()
    : this(new RealClock()) { }
internal PasswordManager(IClock clock)
{
    this.clock = clock;
}
```

This allows me to use a real clock (an implementation that just returns `System.DateTime.Now`) in production, but mock out the clock with something under the control of the unit tests when testing.

## TDD or not to TDD?
The commits here are a pretty good example of my normal programming habits: A mixture of writing first interfaces, then tests, then an implementation (strict TDD), as well as writing implementations first and then tests afterwards. For example, the first four commits have no implementation, just tests. Then, in [commit 5](https://github.com/MartinEden/quondam/commit/8e965337208db4b5dd577085e6766d39dcbe06c7) I implement `PasswordManager`. In doing so, I discover that I need some helper classes (`UserStore` and `PasswordRecord`). So in [commit 6](https://github.com/MartinEden/quondam/commit/88af2cb957d243d63247a98c07d367afbd234dc5) I then write tests for these new classes.

I find this mixed approach works well, giving me both formality and upfront contracts with tests, but also the flexibility to sometimes discover through exploration.