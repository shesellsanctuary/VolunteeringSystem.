using System;
using NUnit.Framework;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Tests
{
    [TestFixture]
    public class CredentialsEmailSetterShould
    {
        private const string ValidEmail = "bernardo@sulzbach.com";

        [Test]
        public void AcceptNull()
        {
            var unused = new Credentials {email = null};
        }

        [Test]
        public void NotAcceptEmptyString()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Credentials {email = ""};
            });
        }

        [Test]
        public void NotAcceptInvalidStrings()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Credentials {email = "·"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Credentials {email = "Oi"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Credentials {email = "10"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Credentials {email = "Bernardo Sulzbach"};
            });
        }

        [Test]
        public void AcceptValidStrings()
        {
            Assert.DoesNotThrow(() =>
            {
                var unused = new Credentials {email = ValidEmail};
            });
        }

        [Test]
        public void ActuallySetTheValue()
        {
            var credentials = new Credentials();
            Assert.DoesNotThrow(() => { credentials.email = ValidEmail; });
            Assert.AreEqual(credentials.email, ValidEmail);
        }
    }
}