using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MartinEden.Quondam.Tests
{
    public class PasswordManagerTests
    {
        private IPasswordManager manager;
        private FakeClock clock;
        private const string user = "foo";

        [SetUp]
        public void CreateManager()
        {
            clock = new FakeClock(DateTime.Now);
            manager = new PasswordManager(clock, TestConstants.HashStrength);
        }

        [Test]
        public void CanGeneratePassword()
        {
            string password = manager.GenerateOneTimePassword(user);
            Assert.IsNotNull(password);
            Assert.AreEqual(16, password.Length);
        }

        [Test]
        public void PasswordsAreDistinct()
        {
            // This cannot be proven in a strict sense, unless we keep generating passwords forever.
            // However, we can at least check that the manager isn't just returning the same string
            // every time.
            var seen = new HashSet<string>();
            foreach (int i in Enumerable.Range(0, 10))
            {
                string password = manager.GenerateOneTimePassword(i.ToString());
                CollectionAssert.DoesNotContain(seen, password);
                seen.Add(password);
            }
        }

        [Test]
        public void GeneratedPasswordValidates()
        {
            string password = manager.GenerateOneTimePassword(user);
            Assert.IsTrue(manager.ValidatePassword(user, password));
        }

        [Test]
        public void PasswordCanOnlyBeUsedOnce()
        {
            string password = manager.GenerateOneTimePassword(user);
            Assert.IsTrue(manager.ValidatePassword(user, password));
            Assert.IsFalse(manager.ValidatePassword(user, password));
        }

        [Test]
        public void OtherPasswordDoesNotValidate()
        {
            string good = manager.GenerateOneTimePassword(user);
            string bad = good.Replace(good[0], (char)(good[0] + 1));
            Assert.IsFalse(manager.ValidatePassword(user, bad));
            Assert.IsTrue(manager.ValidatePassword(user, good));
        }

        [Test]
        public void CannotUseAnotherUsersPassword()
        {
            string passwordA = manager.GenerateOneTimePassword("a");
            string passwordB = manager.GenerateOneTimePassword("b");
            Assert.IsFalse(manager.ValidatePassword("a", passwordB));
            Assert.IsFalse(manager.ValidatePassword("b", passwordA));

            Assert.IsTrue(manager.ValidatePassword("a", passwordA));
            Assert.IsTrue(manager.ValidatePassword("b", passwordB));
        }

        [Test]
        public void GeneratingASecondPasswordInvalidatesThePreviousOne()
        {
            string first = manager.GenerateOneTimePassword(user);
            string second = manager.GenerateOneTimePassword(user);
            Assert.IsFalse(manager.ValidatePassword(user, first));
            Assert.IsTrue(manager.ValidatePassword(user, second));
        }

        [TestCase(0, ExpectedResult = true)]
        [TestCase(15, ExpectedResult = true)]
        [TestCase(30, ExpectedResult = true)]
        [TestCase(30.001, ExpectedResult = false)]
        [TestCase(60, ExpectedResult = false)]
        public bool PasswordExpiresAfterThirtySeconds(double elapsedSeconds)
        {
            string password = manager.GenerateOneTimePassword(user);
            clock.Advance(TimeSpan.FromSeconds(elapsedSeconds));
            return manager.ValidatePassword(user, password);
        }
    }
}