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
    public class ConfirmRabbitMqDeliveryTests
    {
        private Mock<BasicDeliverEventArgs> mockDelivery;
        private Mock<IModel> mockChannel;

        [SetUp]
        public void Setup()
        {
            mockDelivery = new Mock<BasicDeliverEventArgs>();
            mockChannel = new Mock<IModel>();

            mockDelivery.Object.DeliveryTag = 1234;
        }

        [Test]
        public void ConfirmRabbitMqDeliveryCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new ConfirmRabbitMqDeliveryCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("InputObject")]
        [TestCase("Channel")]
        [TestCase("Ack")]
        [TestCase("Nack")]
        [TestCase("Requeue")]
        public void ConfirmRabbitMqDeliveryCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new ConfirmRabbitMqDeliveryCommand();

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }

        [Test]
        public void ConfirmRabbitMqDeliveryCommand_ShouldCallBasicAckOnce()
        {
            var cmdlet = new ConfirmRabbitMqDeliveryCommand(){
                InputObject = mockDelivery.Object,
                Channel = mockChannel.Object,
                Ack = true
            };

            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            mockChannel.Verify(x => x.BasicAck(1234, false), Times.Once);
            mockChannel.VerifyNoOtherCalls();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ConfirmRabbitMqDeliveryCommand_ShouldCallBasicNackOnce(bool requeue)
        {
            var cmdlet = new ConfirmRabbitMqDeliveryCommand(){
                InputObject = mockDelivery.Object,
                Channel = mockChannel.Object,
                Nack = true,
                Requeue = requeue
            };

            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            mockChannel.Verify(x => x.BasicNack(1234, false, requeue), Times.Once);
            mockChannel.VerifyNoOtherCalls();
        }
    }
}