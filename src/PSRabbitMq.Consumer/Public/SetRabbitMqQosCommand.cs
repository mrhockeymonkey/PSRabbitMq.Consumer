using System;
using System.Management.Automation;
using RabbitMQ.Client;

namespace PSRabbitMq.Consumer.Public
{
    [Cmdlet(VerbsCommon.Set, "RabbitMqQos")]
    [OutputType(typeof(IModel))]
    public class SetRabbitMqQosCommand : Cmdlet
    {
        [Parameter(Mandatory=true, ValueFromPipeline=true)]
        public IModel Channel { get; set; }

        [Parameter()]
        public UInt32 PrefetchSize { get; set;} = 0;

        [Parameter()]
        public UInt16 PrefetchCount { get; set; } = 0;

        [Parameter()]
        public bool Global { get; set; } = false;

        [Parameter()]
        public SwitchParameter PassThru { get; set; }

        protected override void EndProcessing()
        {
            Channel.BasicQos(PrefetchSize, PrefetchCount, Global);
            WriteVerbose($"Qos set to PrefetchSiize={PrefetchSize}, PrefetchCount={PrefetchCount}, Global={Global}");

            if (PassThru.IsPresent)
            {
                WriteObject(Channel);
            }
        }
    }
}
