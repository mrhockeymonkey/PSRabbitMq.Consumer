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
    public class SetRabbitMqQosTests
    {
        Mock<IModel> mockChannel;
        
        [SetUp]
        public void Setup()
        {
            mockChannel = new Mock<IModel>();
        }

        [Test]
        public void SetRabbitMqQosCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new SetRabbitMqQosCommand();

            // Act

            // Assert
            Assert.That(cmdlet is Cmdlet, Is.True);
        }

        [TestCase("Channel")]
        [TestCase("PrefetchSize")]
        [TestCase("PrefetchCount")]
        [TestCase("Global")]
        [TestCase("PassThru")]
        public void SetRabbitMqQosCommand_HasCorrectParameters(String parameter)
        {
            // Arrange
            var cmdlet = new SetRabbitMqQosCommand();

            // Act

            // Assert
            Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
        }

        [TestCase((uint)0, (ushort)0, false)]
        [TestCase((uint)1, (ushort)1, true)]
        public void SetRabbitMqQosCommand_CallsBasicQosOnce(UInt32 size, UInt16 count, bool global)
        {
            // Arrange
            var cmdlet = new SetRabbitMqQosCommand(){
                Channel = mockChannel.Object,
                PrefetchSize = size,
                PrefetchCount = count,
                Global = global
            };

            // Act 
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();

            // Assert
            mockChannel.Verify(x => x.BasicQos(size, count, global), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetRabbitMqQosCommand_ChannelReturnedWhenPassThru(bool passThru)
        {
            // Arrange
            var cmdlet = new SetRabbitMqQosCommand(){
                Channel = mockChannel.Object,
                PrefetchSize = 0,
                PrefetchCount = 0,
                Global = true,
                PassThru = passThru
            };

            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            if (passThru)
            {
                Assert.That(result is IModel, Is.True);
            } 
            else
            {
                Assert.That(result, Is.Null);
            }
        }

    }
}