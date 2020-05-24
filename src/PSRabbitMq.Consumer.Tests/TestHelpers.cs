using System;
using System.Management.Automation;

namespace PSRabbitMq.Consumer.Tests
{
    public class TestHelpers
    {
        public static bool HasParameterAttribute(Cmdlet cmdlet, String parameter)
        {
            var property = cmdlet.GetType().GetProperty(parameter);
            
            if (property == null)
            {
                return false;
            }
            else {
                var hasAttribute = Attribute.IsDefined(property, typeof(ParameterAttribute));
                return hasAttribute;
            }
        }
    }
}