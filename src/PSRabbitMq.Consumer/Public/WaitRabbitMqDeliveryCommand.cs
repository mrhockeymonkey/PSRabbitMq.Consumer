using System;
using System.Management.Automation;
using System.Threading;
using RabbitMQ.Client.Events;

namespace PSRabbitMq.Consumer.Public
{
    
    [Cmdlet(VerbsLifecycle.Wait, "RabbitMqDelivery")]
    public class WaitRabbitMqDeliveryCommand : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline=true)]
        public QueueingBasicConsumer Consumer { get; set; }

        [Parameter()]
        public int WaitIntervalSeconds { get; set; } = 1;

        [Parameter()]
        public int Timeout { get; set; } = 0;

        private BasicDeliverEventArgs message;
        private DateTime idleStart;

        public WaitRabbitMqDeliveryCommand() {}
        
        public WaitRabbitMqDeliveryCommand(QueueingBasicConsumer consumer) => Consumer = consumer;

        
        private void ThrowOnTimeout()
        {
            // timeout=0 is equivalent to never timeout
            if (Timeout == 0)
            {
                return;
            }

            // calculate the seconds passed since beocming idle
            DateTime idleNow = DateTime.Now;
            TimeSpan idleDuration = (idleNow - idleStart);

            // throw is idle time has exceeded timeout
            if (idleDuration.TotalSeconds > Timeout)
            {
                throw new TimeoutException();
            }
        }
        
        protected override void EndProcessing()
        {
            idleStart = DateTime.Now;
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
                        this.WriteVerbose("Queue is empty, waiting for new items...");
                        queueEmpty = true;
                        idleStart = DateTime.Now;
                    }

                    WriteDebug($"Sleeping for {WaitIntervalSeconds} seconds");
                    Thread.Sleep(1000 * WaitIntervalSeconds);
                    ThrowOnTimeout();
                }
            }
        }
    }
}
