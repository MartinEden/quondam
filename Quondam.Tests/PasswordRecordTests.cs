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
            var record = new PasswordRecord("abc", noon, TestConstants.HashStrength);
            return record.IsFresh(noon + TimeSpan.FromSeconds(elapsed));
        }

        [Test]
        public void RecordOnlyValidatesPasswordItWasCreatedWith()
        {
            var record = new PasswordRecord("a", DateTime.Now, TestConstants.HashStrength);
            Assert.IsTrue(record.ValidatePassword("a"));
            Assert.IsFalse(record.ValidatePassword("b"));
            Assert.IsFalse(record.ValidatePassword(""));
        }

        /// <summary>
        /// I'm influenced here by http://security.stackexchange.com/questions/3959/recommended-of-iterations-when-using-pkbdf2-sha256.
        /// 
        /// In considering the number of rounds of hashing to perform when using PBKDF2 (as implemented in C# by
        /// System.Security.Cryptography.by Rfc2898DeriveBytes) there are two basic considerations:
        /// 1. How much can I afford, before system performance / user experience is adversely affected?
        /// 2. Once I have I have afforded everything I can, is this enough to deter an attacker? 
        ///    If not, I need to upgrade algorithms / hardware / etc.
        ///    
        /// In this coding challenge, I can't make sensible decisions about the characteristics of an attacker, so I have chosen to
        /// focus on (1). I chose a delay of 500 - 750ms as the maximum acceptable range before user experience is degraded too much.
        /// This test checks that we have set the number of hashing rounds correctly to achieve the desired performance.
        /// 
        /// Note that in a real system, we would have to make sure this test was run on the production machine, not the build server,
        /// as they would have difference performance characteristics.
        /// </summary>
        [Test]
        public void HashingPasswordsCostsTheRightAmount()
        {
            const string password = "b7qKH1p-7+1wP|!+";
            var start = DateTime.Now;
            new PasswordRecord(password, start, PasswordManager.DefaultHashStrength);
            var elapsed = (DateTime.Now - start).TotalMilliseconds;
            Assert.Greater(elapsed, 500);
            Assert.Less(elapsed, 750);
        }
    }
}
