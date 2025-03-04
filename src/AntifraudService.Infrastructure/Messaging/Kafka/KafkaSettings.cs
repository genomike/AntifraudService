using Microsoft.Extensions.Configuration;

namespace AntifraudService.Infrastructure.Messaging.Kafka
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }

        public static KafkaSettings LoadFromConfiguration(IConfiguration configuration)
        {
            var kafkaSettings = new KafkaSettings();
            configuration.GetSection("Kafka").Bind(kafkaSettings);
            return kafkaSettings;
        }
    }
}