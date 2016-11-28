using NUnit.Framework;
using System;

namespace MartinEden.Quondam.Tests
{
    public class UserStoreTests
    {
        private UserStore store;
        private const string user = "user";

        [SetUp]
        public void CreateStore()
        {
            store = new UserStore();
        }

        [Test]
        public void GetPasswordReturnsNullForUnknownUser()
        {
            Assert.IsNull(store.GetPassword(user));
        }

        [Test]
        public void GetPasswordReturnsStoredPassword()
        {
            string password = "swordfish";
            var time = DateTime.Today;
            store.StorePassword(user, password, time);
            var record = store.GetPassword(user);
            checkRecord(password, time, record);
        }

        [Test]
        public void AfterClearPasswordGetPasswordReturnsNull()
        {
            string password = "swordfish";
            var time = DateTime.Today;
            store.StorePassword(user, password, time);
            store.ClearPassword(user);
            Assert.IsNull(store.GetPassword(user));
        }

        [Test]
        public void CanStorePasswordsForMultipleUsers()
        {
            string password1 = "cake";
            string password2 = "cherry";
            string user1 = "alice";
            string user2 = "bob";
            var time1 = DateTime.Now;
            var time2 = time1.AddSeconds(1);

            store.StorePassword(user1, password1, time1);
            store.StorePassword(user2, password2, time2);
            checkRecord(password1, time1, store.GetPassword(user1));
            checkRecord(password2, time2, store.GetPassword(user2));
        }

        [Test]
        public void StoringNewPasswordOverridesPrevious()
        {
            string password1 = "hunter13";
            string password2 = "fsdfeergf4";
            var time1 = DateTime.Now;
            var time2 = time1.AddSeconds(1);

            store.StorePassword(user, password1, time1);
            store.StorePassword(user, password2, time2);
            var record = store.GetPassword(user);
            checkRecord(password2, time2, record);
        }

        private static void checkRecord(string expectedPassword, DateTime expectedTimestamp, PasswordRecord record)
        {
            Assert.IsNotNull(record);
            Assert.IsTrue(record.ValidatePassword(expectedPassword));
            Assert.AreEqual(expectedTimestamp, record.Timestamp);
        }
    }
}
