using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using System.Management.Automation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PSRabbitMq.Consumer.Public;

namespace PSRabbitMq.Consumer.Tests.Public
{
    [TestFixture]
    public class ConvertFromRabbitMqDeliveryTests
    {
        Mock<BasicDeliverEventArgs> mockDelivery;

        [SetUp]
        public void Setup()
        {
            mockDelivery = new Mock<BasicDeliverEventArgs>();
            mockDelivery.Object.DeliveryTag = 1234;
        }

        [Test]
        public void ConvertFromRabbitMqDeliveryCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new ConvertFromRabbitMqDeliveryCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("InputObject")]
        public void ConvertFromRabbitMqDeliveryCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new ConvertFromRabbitMqDeliveryCommand();

            // Act

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }

        [Test]
        public void ConvertFromRabbitMqDeliveryCommand_ReturnsDecodedString()
        {
            // Arrange
            var bytes = System.Text.Encoding.UTF8.GetBytes("Hello");
            mockDelivery.Object.Body = bytes;
            var cmdlet = new ConvertFromRabbitMqDeliveryCommand(){
                InputObject = mockDelivery.Object
            };

            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.That(result, Is.EqualTo("Hello"));
        }
    }
}