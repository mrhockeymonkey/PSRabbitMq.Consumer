using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer
{
    [Cmdlet(VerbsCommon.New, "RabbitMqChannel")]
    [OutputType(typeof(IModel))]
    public class NewRabbitMqChannel : PSCmdlet
    {
        [Parameter(Mandatory=true)]
        public IConnection Connection { get; set; }


        protected override void EndProcessing()
        {
            this.WriteVerbose($"Creating new RabbitMQ channel");
            IModel channel = Connection.CreateModel();
            
            this.WriteObject(channel);
        }
    }
}
