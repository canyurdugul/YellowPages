using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Queue.Common.Abstract;

namespace YellowPages.Queue.Common.Concrete
{
    public class MessageBusCommonService : IMessageBusService
    {

        private readonly ILogger<MessageBusCommonService> _logger;
        private readonly IConfiguration _configuration;

        public MessageBusCommonService(
            ILogger<MessageBusCommonService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IConnection GetConnection()
        {
            IConnection connection = null;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.GetValue<string>("MessageBusConfiguration:HostName"),
                    UserName = _configuration.GetValue<string>("MessageBusConfiguration:UserName"),
                    Password = _configuration.GetValue<string>("MessageBusConfiguration:Password"),
                };

                factory.TopologyRecoveryEnabled = false;
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

                connection = factory.CreateConnection();
            }
            catch (BrokerUnreachableException bex)
            {
                _logger.LogError($"{nameof(MessageBusCommonService)} connection start error", bex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(MessageBusCommonService)} connection start error", ex);
            }

            return connection;
        }

        public IModel GetModel(IConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(IConnection));
            }

            return connection.CreateModel();
        }
    }
}
