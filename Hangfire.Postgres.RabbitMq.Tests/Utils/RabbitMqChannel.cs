using System.Collections.Generic;
using RabbitMQ.Client;

namespace Hangfire.Postgres.RabbitMq.Tests.Utils
{
    public class RabbitMqChannel
    {
        private readonly IEnumerable<string> _queues;

        public RabbitMqChannel(IEnumerable<string> queues)
        {
            _queues = queues;
            ConnectionFactory = new ConnectionFactory { HostName = "localhost" };
        }

        public ConnectionFactory ConnectionFactory { get; private set; }

        public RabbitMqJobQueue CreateQueue()
        {
            return new RabbitMqJobQueue(_queues, ConnectionFactory, null);
        }
    }
}