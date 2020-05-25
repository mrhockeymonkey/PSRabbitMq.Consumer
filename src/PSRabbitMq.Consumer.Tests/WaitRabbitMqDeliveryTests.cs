using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer.Tests
{
    [TestFixture]
    public class WaitRabbitMqDeliveryTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void WaitRabbitMqDeliveryCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new WaitRabbitMqDeliveryCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("Consumer")]
        [TestCase("WaitIntervalSecondss")]
        public void WaitRabbitMqDeliveryCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new WaitRabbitMqDeliveryCommand();

            // Act

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }
    }
}