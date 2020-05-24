using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PSRabbitMq.Consumer
{
    [Cmdlet(VerbsLifecycle.Confirm, "RabbitMqDelivery")]
    public class ConfirmRabbitMqDeliveryCommand : Cmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public BasicDeliverEventArgs InputObject;

        [Parameter(Mandatory=true)]
        public IModel Channel;

        [Parameter(ParameterSetName="Ack")]
        public SwitchParameter Ack;

        [Parameter(ParameterSetName="Nack")]
        public SwitchParameter Nack;

        [Parameter(ParameterSetName="Nack")]
        public SwitchParameter Requeue;

        private const bool Multiple = false;

        protected override void ProcessRecord()
        {
            var deliveryTag = InputObject.DeliveryTag;
            if (Ack.IsPresent)
            {
                Channel.BasicAck(deliveryTag, Multiple);
                WriteVerbose($"Ack sent for delivery tag {deliveryTag}");
            }
            else if (Nack.IsPresent)
            {
                Channel.BasicNack(deliveryTag, Multiple, Requeue.ToBool());
                WriteVerbose($"Nack sent for delivery tag {deliveryTag} (Requeued: {Requeue.ToBool()})");
            }
        }
    }
}
