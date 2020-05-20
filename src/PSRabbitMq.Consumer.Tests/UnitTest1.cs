using System;
using System.Collections;
using NUnit.Framework;
using System.Management.Automation;

namespace PSRabbitMq.Consumer.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldReturnFoo()
        {
            // Arrange
            var cmdlet = new GetSomething();
            
            // Act.
            IEnumerator enumerator = cmdlet.Invoke().GetEnumerator(); //.OfType<string>(); //.ToList();
            
            // Assert.
            Assert.That(cmdlet is Cmdlet);
            Assert.True(enumerator.MoveNext());
            Assert.That(enumerator.Current.ToString(), Is.EqualTo("foo"));

            //TestContext.Progress.WriteLine(enumerator.Current);

        }
    }
}