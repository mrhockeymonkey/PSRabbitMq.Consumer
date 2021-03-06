using System;
using System.Management.Automation;
using System.Text;
using RabbitMQ.Client.Events;

namespace PSRabbitMq.Consumer.Public
{

    [Cmdlet(VerbsData.ConvertFrom, "RabbitMqDelivery")]
    [OutputType(typeof(String))]
    public class ConvertFromRabbitMqDeliveryCommand : Cmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public BasicDeliverEventArgs InputObject { get; set; }

        protected override void ProcessRecord()
        {
            var decoded = Encoding.UTF8.GetString(InputObject.Body);
            this.WriteObject(decoded);
        }
    }
}