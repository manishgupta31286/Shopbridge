using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shopbridge_base.Domain.Models;

namespace Shopbridge_base.Domain.Services.Implementations
{
    public interface IEventEmitter
    {
        void Publish(Product product);
    }

    public class AmqpEventEmitter: IEventEmitter
    {
        private AmqpOptions rabbitOptions;

        private ConnectionFactory connectionFactory;

        public AmqpEventEmitter(IOptions<AmqpOptions> amqpOptions)
        {
            this.rabbitOptions = amqpOptions.Value;
            connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = rabbitOptions.Hostname;
        }

        public void Publish(Product product)
        {
            using (IConnection conn = connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "shopbridgequeue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                    var body = Encoding.UTF8.GetBytes(data);

                    channel.BasicPublish(exchange: "", 
                        routingKey: "shopbridgequeue", 
                        basicProperties: null, 
                        body: body);
                }
            }
        }
    }
}
