﻿using System;
using NUnit.Framework;
using VolunteeringSystem.Models;

namespace VolunteeringSystem.Tests
{
    [TestFixture]
    public class PersonCpfSetterShould
    {
        [Test]
        public void AcceptNull()
        {
            var unused = new Person {CPF = null};
        }

        [Test]
        public void NotAcceptEmptyString()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Person {CPF = ""};
            });
        }

        [Test]
        public void NotAcceptInvalidStrings()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Person {CPF = "Oi"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Person {CPF = "10"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Person {CPF = "546.760.405-85"};
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var unused = new Person {CPF = "546.760.405-50"};
            });
        }

        [Test]
        public void AcceptValidStrings()
        {
            Assert.DoesNotThrow(() =>
            {
                var unused = new Person {CPF = "54676040580"};
            });
            Assert.DoesNotThrow(() =>
            {
                var unused = new Person {CPF = "546760405-80"};
            });
            Assert.DoesNotThrow(() =>
            {
                var unused = new Person {CPF = "546.760.405-80"};
            });
        }

        [Test]
        public void ActuallySetTheValue()
        {
            var unused = new Person();
            Assert.DoesNotThrow(() => { unused.CPF = "54676040580"; });
            Assert.AreEqual(unused.CPF, "546.760.405-80");
        }

        [Test]
        public void StripDashes()
        {
            var unused = new Person();
            Assert.DoesNotThrow(() => { unused.CPF = "546760405-80"; });
            Assert.AreEqual(unused.CPF, "546.760.405-80");
        }

        [Test]
        public void StripAllPunctuation()
        {
            var person = new Person();
            Assert.DoesNotThrow(() => { person.CPF = "546.760.405-80"; });
            Assert.AreEqual(person.CPF, "546.760.405-80");
        }
    }
}