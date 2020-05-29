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
    public class StartRabbitMqConsumerTests
    {
        private Mock<IModel> mockChannel;
        private QueueingBasicConsumer consumer;

        [SetUp]
        public void Setup()
        {
            mockChannel = new Mock<IModel>();
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

        [Test]
        public void StartRabbitMqConsumerCommand_HasDefaultsParameters()
        {
            // Arrange
            var cmdlet = new StartRabbitMqConsumerCommand();

            // Act

            // Assert
            Assert.That(cmdlet.AutoAck, Is.False);
            Assert.That(cmdlet.Tag, Is.EqualTo(String.Empty));
            Assert.That(cmdlet.NoLocal, Is.False);
            Assert.That(cmdlet.Exclusive, Is.False);
            Assert.That(cmdlet.Arguments, Is.Null);
        }

        [Test]
        public void StartRabbitMqConsumerCommand_UsesSpecifiedParameters()
        {
            // Arrange
            var arguments = new Dictionary<String,Object>(){
                ["foo"] = "bar"
            };
            mockChannel.Setup(x => x.BasicConsume("SomeQueue", true, "SomeTag", true, true, arguments, It.IsAny<QueueingBasicConsumer>()));
            var cmdlet = new StartRabbitMqConsumerCommand(){
                Channel = mockChannel.Object,
                QueueName = "SomeQueue",
                AutoAck = true,
                Tag = "SomeTag",
                NoLocal = true,
                Exclusive = true,
                Arguments = arguments
            };
            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;


            // Assert
            Assert.That(cmdlet.QueueName, Is.EqualTo("SomeQueue"));
            Assert.That(cmdlet.AutoAck, Is.True);
            Assert.That(cmdlet.Tag, Is.EqualTo("SomeTag"));
            Assert.That(cmdlet.NoLocal, Is.True);
            Assert.That(cmdlet.Exclusive, Is.True);
            Assert.That(cmdlet.Arguments, Is.EqualTo(arguments));
            mockChannel.Verify(x => x.BasicConsume("SomeQueue", true, "SomeTag", true, true, arguments, It.IsAny<QueueingBasicConsumer>()), Times.Once);
        }

        [Test]
        public void StartRabbitMqConsumerCommand_ReturnsConsumerObject()
        {
            // Arrange
            var cmdlet = new StartRabbitMqConsumerCommand(){
                Channel = mockChannel.Object,
                QueueName = "SomeOtherQueue"
            };

            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result is QueueingBasicConsumer, Is.True);
        }
    }
}