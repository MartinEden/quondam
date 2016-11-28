using NUnit.Framework;
using System;

namespace MartinEden.Quondam.Tests
{
    public class PasswordRecordTests
    {
        [TestCase(0, ExpectedResult = true)]
        [TestCase(15, ExpectedResult = true)]
        [TestCase(30, ExpectedResult = true)]
        [TestCase(30.001, ExpectedResult = false)]
        [TestCase(60, ExpectedResult = false)]
        public bool RecordsOnlyRemainFreshForThirtySeconds(double elapsed)
        {
            var noon = DateTime.Today.AddHours(12);
            var record = new PasswordRecord("abc", noon);
            return record.IsFresh(noon + TimeSpan.FromSeconds(elapsed));
        }

        [Test]
        public void RecordOnlyValidatesPasswordItWasCreatedWith()
        {
            var record = new PasswordRecord("a", DateTime.Now);
            Assert.IsTrue(record.ValidatePassword("a"));
            Assert.IsFalse(record.ValidatePassword("b"));
            Assert.IsFalse(record.ValidatePassword(""));
        }
    }
}
