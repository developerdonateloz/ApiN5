using Confluent.Kafka;

namespace ApiN5.Consumers
{
    public class KafkaConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }

        public void Consume(Action<ConsumeResult<string, string>> callback)
        {
            while (true)
            {
                var result = _consumer.Consume();
                callback(result);
            }
        }

    }
}
