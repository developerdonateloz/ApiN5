using Confluent.Kafka;

namespace ApiN5.Producers
{
    public class KafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync(string topic, string message)
        {
            var result = await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Value = message
            });

            Console.WriteLine($"Produced message");
        }
    }
}
