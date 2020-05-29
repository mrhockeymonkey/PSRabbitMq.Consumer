using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using System.Management.Automation;
using RabbitMQ.Client;
using PSRabbitMq.Consumer.Public;

namespace PSRabbitMq.Consumer.Tests.Public
{
    [TestFixture]
    public class NewRabbitMqChannelTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void NewRabbitMqChannelCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new NewRabbitMqChannelCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("Connection")]
        public void NewRabbitMqChannelCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new NewRabbitMqChannelCommand();

            // Act

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }
    }
}