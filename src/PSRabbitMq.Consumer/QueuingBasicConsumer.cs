using System;
using System.Collections.Generic;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace PSRabbitMq.Consumer
{

    public class QueueingBasicConsumer : DefaultBasicConsumer
    {
        public Queue<BasicDeliverEventArgs> Queue = new Queue<BasicDeliverEventArgs>();

        public QueueingBasicConsumer(IModel model) : base(model)
        {

        }

        public override  void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            var eventArgs = new BasicDeliverEventArgs
            {
                ConsumerTag = consumerTag,
                DeliveryTag = deliveryTag,
                Redelivered = redelivered,
                Exchange = exchange,
                RoutingKey = routingKey,
                BasicProperties = properties,
                Body = body
            };
            Queue.Enqueue(eventArgs);
        }
    }
}
