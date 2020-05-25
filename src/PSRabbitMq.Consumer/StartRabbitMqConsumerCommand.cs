using System;
using System.Management.Automation;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer
{
    [Cmdlet(VerbsLifecycle.Start, "RabbitMqConsumer")]
    [OutputType(typeof(QueueingBasicConsumer))]
    public class StartRabbitMqConsumerCommand : Cmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public IModel Channel { get; set; }

        [Parameter(Mandatory = true)]
        public String QueueName { get; set; }

        [Parameter()]
        public bool AutoAck { get; set; } = false;

        [Parameter()]
        public String Tag { get; set; } = String.Empty;

        [Parameter()]
        public bool NoLocal { get; set; } = false;

        [Parameter()]
        public bool Exclusive { get; set; } = false;

        [Parameter()]
        public IDictionary<String,Object> Arguments { get; set; } = null;

        private QueueingBasicConsumer Consumer;

        protected override void EndProcessing()
        {
            Consumer = new QueueingBasicConsumer(Channel);
            Channel.BasicConsume(QueueName, AutoAck, Tag, NoLocal, Exclusive, Arguments, Consumer);
            WriteVerbose($"Created new consumer for queue '{QueueName}' with tag {Consumer.ConsumerTag}");
            WriteObject(Consumer);
        }
    }
}
