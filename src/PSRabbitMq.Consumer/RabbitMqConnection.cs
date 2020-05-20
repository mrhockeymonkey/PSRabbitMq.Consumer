using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer
{
    [Cmdlet(VerbsCommon.New, "RabbitMqConnection")]
    [OutputType(typeof(IConnection))]
    public class NewRabbitMqConnection : PSCmdlet
    {
        [Parameter(Mandatory=true)]
        public string HostName { get; set; }

        [Parameter()]
        public int Port { get; set;} = 5672;

        [Parameter()]
        public string VirtualHost { get; set; } = "/";


        protected override void EndProcessing()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = HostName;
            factory.Port = Port;
            factory.VirtualHost = VirtualHost;

            this.WriteVerbose($"Creating new RabbitMQ connection to {HostName}");
            IConnection conn = factory.CreateConnection();
            this.WriteObject(conn);
            base.EndProcessing();
        }
    }
}
