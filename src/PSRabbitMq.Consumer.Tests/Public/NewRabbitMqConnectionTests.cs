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
    public class NewRabbitMqConnectionTests
    {
        Mock<ConnectionFactory> mockConnectionFactory;
        Mock<IConnection> mockConnection;

        [SetUp]
        public void Setup()
        {
            mockConnectionFactory = new Mock<ConnectionFactory>();
            mockConnection = new Mock<IConnection>();

            mockConnection.Setup(x => x.IsOpen).Returns(true);
            mockConnectionFactory.Setup(x => x.CreateConnection()).Returns(mockConnection.Object);
        }

        [Test]
        public void NewRabbitMqConnectionCommand_IsCmdlet()
        {
            // Arrange
            var cmdlet = new NewRabbitMqConnectionCommand();

            // Act
            
            // Assert
            Assert.That(cmdlet is Cmdlet);
        }

        [Test]
        public void NewRabbitMqConnectionCommand_HasCorrectParameters()
        {
            // Arrange
            var cmdlet = new NewRabbitMqConnectionCommand();
            var expectedParameters = new List<String> {"HostName", "Port", "VirtualHost", "UserName", "Password"};
            
            // Act
            foreach (var parameter in expectedParameters)
            {
                // Assert.
                Assert.That(TestHelpers.HasParameterAttribute(cmdlet, parameter), Is.True);
            }
        }

        [Test]
        public void NewRabbitMqConnectionCommand_ReturnsNewConnection()
        {
            // Arrange
            var cmdlet = new NewRabbitMqConnectionCommand(mockConnectionFactory.Object);

            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            mockConnectionFactory.Verify(x => x.CreateConnection(), Times.Once);
            Assert.That(result is IConnection);
        }

        [Test]
        public void NewRabbitMqConnectionCommand_HasDefaultParameters()
        {
            // Arrange
            var cmdlet = new NewRabbitMqConnectionCommand();

            // Act

            // Assert
            Assert.That(cmdlet.Port, Is.EqualTo(5672));
            Assert.That(cmdlet.VirtualHost, Is.EqualTo("/"));
            Assert.That(cmdlet.UserName, Is.EqualTo("guest"));
            Assert.That(cmdlet.Password, Is.EqualTo("guest"));
        }

        [Test]
        public void NewRabbitMqConnectionCommand_UsesSpecifiedParameters()
        {
            // Arrange
            var cmdlet = new NewRabbitMqConnectionCommand(mockConnectionFactory.Object){
                HostName = "someHost",
                Port = 1234,
                VirtualHost = "someVirtualHost",
                UserName = "someUserName",
                Password = "somePassword"
            };

            // Act
            var enumerator = cmdlet.Invoke().GetEnumerator();
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.That(cmdlet.HostName, Is.EqualTo("someHost"));
            Assert.That(cmdlet.Port, Is.EqualTo(1234));
            Assert.That(cmdlet.VirtualHost, Is.EqualTo("someVirtualHost"));
            Assert.That(cmdlet.UserName, Is.EqualTo("someUserName"));
            Assert.That(cmdlet.Password, Is.EqualTo("somePassword"));

            Assert.That(cmdlet.factory.HostName, Is.EqualTo("someHost"));
            Assert.That(cmdlet.factory.Port, Is.EqualTo(1234));
            Assert.That(cmdlet.factory.VirtualHost, Is.EqualTo("someVirtualHost"));
            Assert.That(cmdlet.factory.UserName, Is.EqualTo("someUserName"));
            Assert.That(cmdlet.factory.Password, Is.EqualTo("somePassword"));
        }
    }
}