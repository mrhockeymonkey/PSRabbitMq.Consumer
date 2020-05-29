using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer.Public
{
    [Cmdlet(VerbsCommon.New, "RabbitMqConnection")]
    [OutputType(typeof(IConnection))]
    public class NewRabbitMqConnectionCommand : Cmdlet
    {
        [Parameter(Mandatory=true, Position=0)]
        public string HostName { get; set; }

        [Parameter()]
        public int Port { get; set;} = 5672;

        [Parameter()]
        public string VirtualHost { get; set; } = "/";

        [Parameter()]
        public string UserName { get; set; } = "guest";

        [Parameter()]
        public string Password { get; set; } = "guest";

        public ConnectionFactory factory { get; }
        private IConnection Connection;

        public NewRabbitMqConnectionCommand()
        {
            factory = new ConnectionFactory();
        }

        public NewRabbitMqConnectionCommand(ConnectionFactory connectionFactory)
        {
            factory = connectionFactory;
        }


        protected override void EndProcessing()
        {
            factory.HostName = HostName;
            factory.Port = Port;
            factory.VirtualHost = VirtualHost;
            factory.UserName = UserName;
            factory.Password = Password;

            Connection = factory.CreateConnection();
            this.WriteVerbose($"Created new RabbitMQ connection to {Connection.Endpoint}");
            this.WriteObject(Connection);
            base.EndProcessing();
        }
    }
}
