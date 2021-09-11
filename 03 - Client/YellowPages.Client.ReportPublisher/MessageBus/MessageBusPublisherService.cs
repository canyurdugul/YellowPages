using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Queue.Common.Abstract;

namespace YellowPages.Client.ReportPublisher.MessageBus
{
    public class MessageBusPublisherService : IPublisherService
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBusService _messageBusService;

        public MessageBusPublisherService(IConfiguration configuration, IMessageBusService messageBusService)
        {
            _configuration = configuration;
            _messageBusService = messageBusService;
        }

        public void Enqueue<T>(T queueDataModel) where T : class, new()
        {
            string runnerName = _configuration.GetValue<string>("RunnerSettings:Name");

            using (var connection = _messageBusService.GetConnection())
            using (var channel = _messageBusService.GetModel(connection))
            {
                channel.QueueDeclare(queue: runnerName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.Expiration = (1000 * 60 * 60 * 2).ToString();

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(queueDataModel));

                channel.BasicPublish(exchange: "",
                    routingKey: runnerName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body);
            }
        }

        public uint GetMessageCount(string queueName)
        {
            using (var connection = _messageBusService.GetConnection())
            using (var channel = _messageBusService.GetModel(connection))
            {
                return channel.MessageCount(queueName);
            }
        }
    }
}
