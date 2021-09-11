using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;
using YellowPages.Queue.Common.Abstract;
using YellowPages.Queue.Process.Abstract;

namespace YellowPages.Client.ReportConsumer.MessageBus
{
    public class MessageBusConsumerService : IConsumerService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusConsumerService> _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IProcessService _processService;
        private readonly IReportBusiness _reportBusiness;

        private IModel _channel;
        private IConnection _connection;
        private EventingBasicConsumer _consumer;
        private SemaphoreSlim _consumerSemaphoreSlim;

        public MessageBusConsumerService(IMessageBusService messageBusService, IConfiguration configuration, ILogger<MessageBusConsumerService> logger, IUnitOfWorkFactory unitOfWorkFactory, IProcessService processService, IReportBusiness reportBusiness)
        {
            _messageBusService = messageBusService;
            _processService = processService;
            _configuration = configuration;
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _reportBusiness = reportBusiness;
        }

        public async Task Start()
        {
            string runnerName = _configuration.GetValue<string>("RunnerSettings:Name");

            ushort parallelThreadsCount = _configuration.GetValue<ushort>("MessageBusConfiguration:ParallelThreadsCount");

            _consumerSemaphoreSlim = new SemaphoreSlim(parallelThreadsCount);

            _connection = _messageBusService.GetConnection();

            if (_connection is null)
            {
                throw new ArgumentNullException(nameof(_connection));
            }

            _channel = _messageBusService.GetModel(_connection);

            _channel.QueueDeclare(queue: runnerName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _channel.BasicQos(0, parallelThreadsCount, false);

            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += Consumer_Received;

            await Task.FromResult(_channel.BasicConsume(queue: runnerName,
                                  autoAck: false,
                                  consumer: _consumer));
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                _consumerSemaphoreSlim.Wait();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var model = JsonConvert.DeserializeObject<Report>(message);

                Task.Run(async () =>
                {
                    try
                    {
                        try
                        {
                            _logger.LogInformation($"RequestId : {model.Id} at consumer.");
                            using (var unitOfWork = _unitOfWorkFactory.Create())
                            {
                                var fullPath = await _processService.Execute(unitOfWork);
                                model.ReportStatus = Common.Enums.ReportStatusEnum.Done;
                                model.ReportPath = fullPath;
                                await _reportBusiness.Update(unitOfWork, model);
                                _logger.LogInformation($"RequestId : {model.Id} is done.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"RequestId : {model.Id} failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"{nameof(MessageBusConsumerService)} error");
                    }
                    finally
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                        _consumerSemaphoreSlim.Release();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(MessageBusConsumerService)} error");
            }
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _consumerSemaphoreSlim?.Dispose();
        }
    }
}
