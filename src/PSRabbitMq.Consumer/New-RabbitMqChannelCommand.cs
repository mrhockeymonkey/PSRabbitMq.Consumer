using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer
{
    [Cmdlet(VerbsCommon.New, "RabbitMqChannel")]
    [OutputType(typeof(IModel))]
    public class NewRabbitMqChannelCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public IConnection Connection { get; set; }

        private IModel Channel;

        public NewRabbitMqChannelCommand(){}
        
        public NewRabbitMqChannelCommand(IConnection connection) => Connection = connection;

        
        protected override void EndProcessing()
        {
            Channel = Connection.CreateModel();
            this.WriteVerbose($"Created new RabbitMQ channel number {Channel.ChannelNumber}");
            this.WriteObject(Channel);
        }
    }
}
