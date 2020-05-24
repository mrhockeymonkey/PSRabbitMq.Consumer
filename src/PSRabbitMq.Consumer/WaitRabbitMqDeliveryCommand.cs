using System;
using System.Management.Automation;
using System.Threading;
using RabbitMQ.Client.Events;

namespace PSRabbitMq.Consumer
{
    
    [Cmdlet(VerbsLifecycle.Wait, "RabbitMqDelivery")]
    public class WaitRabbitMqDeliveryCommand : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline=true)]
        public QueueingBasicConsumer Consumer { get; set; }

        [Parameter()]
        public int WaitIntervalSeconds { get; set; } = 1;

        private BasicDeliverEventArgs message;

        public WaitRabbitMqDeliveryCommand() {}
        
        public WaitRabbitMqDeliveryCommand(QueueingBasicConsumer consumer) => Consumer = consumer;

        protected override void EndProcessing()
        {
            var queueEmpty = false;
            WriteVerbose($"Waiting for delivery with interval of {WaitIntervalSeconds} seconds");

            while (true)
            {
                var queuedMessage = Consumer.Queue.TryDequeue(out message);

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

                    WriteDebug($"Sleeping for {WaitIntervalSeconds} seconds");
                    Thread.Sleep(1000 * WaitIntervalSeconds);
                }
            }
        }
    }
}
