using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer.Tests
{
    [TestFixture]
    public class StartRabbitMqConsumerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void StartRabbitMqConsumerCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new StartRabbitMqConsumerCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("Channel")]
        [TestCase("QueueName")]
        [TestCase("AutoAck")]
        [TestCase("Tag")]
        [TestCase("NoLocal")]
        [TestCase("Exclusive")]
        [TestCase("Arguments")]
        public void StartRabbitMqConsumerCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new StartRabbitMqConsumerCommand();

            // Act

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }
    }
}