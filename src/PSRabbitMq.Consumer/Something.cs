using System;
using System.Management.Automation;

namespace PSRabbitMq.Consumer 
{
    [Cmdlet(VerbsCommon.Get, "Something")]
    public class GetSomething : Cmdlet
    {
        protected override void EndProcessing()
        {
            this.WriteVerbose("foo");
            this.WriteObject("foo");
        }
    }
}