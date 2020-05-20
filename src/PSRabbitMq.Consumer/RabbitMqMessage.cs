using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PSRabbitMq.Consumer
{
    
    [Cmdlet(VerbsLifecycle.Wait, "RabbitMqMessage")]
    public class WaitRabbitMqMessage : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public IModel Channel;

        [Parameter(Mandatory = true)]
        public String QueueName;

        [Parameter()]
        public bool AutoAck = false;

        [Parameter()]
        public String ConsumerTag = "";

        [Parameter()]
        public bool NoLocal = false;

        [Parameter()]
        public bool Exclusive = false;

        [Parameter()]
        public IDictionary<String,Object> Arguments = null;

        [Parameter()]
        public int WaitIntervalSeconds = 1;

        private BasicDeliverEventArgs message;

        protected override void EndProcessing()
        {
            this.WriteVerbose("Creating new QueueingBasicConsumer");
            var queueEmpty = false;
            var consumer = new QueueingBasicConsumer(Channel);
            Channel.BasicConsume(QueueName, AutoAck, ConsumerTag, NoLocal, Exclusive, Arguments, consumer);

            while (true)
            {
                var queuedMessage = consumer.Queue.TryDequeue(out message);

                if (queuedMessage)
                {
                    queueEmpty = false;
                    this.WriteObject(message);
                }
                else
                {
                    if (!queueEmpty)
                    {
                        this.WriteVerbose("Queue is now empty, waiting for new items...");
                        queueEmpty = true;
                    }
                    this.WriteVerbose($"Waiting {WaitIntervalSeconds}");
                    Thread.Sleep(1000 * WaitIntervalSeconds);
                }
            }
        }

        protected override void StopProcessing()
        {
            Channel.Close();
        }
    }

    [Cmdlet(VerbsData.ConvertFrom, "RabbitMqMessage")]
    public class ConvertFromRabbitMqMessage : PSCmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public BasicDeliverEventArgs InputObject;

        protected override void ProcessRecord()
        {
            var decoded = Encoding.UTF8.GetString(InputObject.Body);
            this.WriteObject(decoded);
        }
    }

    [Cmdlet(VerbsCommunications.Send, "RabbitMqMessageAck")]
    public class SendRabbitMqMessageAck : PSCmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public BasicDeliverEventArgs InputObject;

        [Parameter(Mandatory=true)]
        public IModel Channel;

        protected override void ProcessRecord()
        {
            Channel.BasicAck(InputObject.DeliveryTag, false);
        }
    }

    [Cmdlet(VerbsCommunications.Send, "RabbitMqMessageNack")]
    public class SendRabbitMqMessageNck : PSCmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public BasicDeliverEventArgs InputObject;

        [Parameter(Mandatory=true)]
        public IModel Channel;

        [Parameter()]
        public SwitchParameter Requeue;

        protected override void ProcessRecord()
        {
            Channel.BasicNack(InputObject.DeliveryTag, false, Requeue.ToBool());
        }
    }
}
