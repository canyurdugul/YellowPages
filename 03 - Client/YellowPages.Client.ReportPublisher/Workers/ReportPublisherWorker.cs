using Fixer.Core.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Common.Enums;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;
using YellowPages.Queue.Common.Abstract;

namespace YellowPages.Client.ReportPublisher.Workers
{
    public class ReportPublisherWorker : ISchedulerTask
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger<ReportPublisherWorker> _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IConfiguration _configuration;
        private readonly IReportBusiness _reportBusiness;

        public ReportPublisherWorker(IPublisherService publisherService, ILogger<ReportPublisherWorker> logger, IUnitOfWorkFactory unitOfWorkFactory, IConfiguration configuration, IReportBusiness reportBusiness)
        {
            _publisherService = publisherService;
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _configuration = configuration;
            _reportBusiness = reportBusiness;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ReportPublisherWorker worked.");

            int limit = _configuration.GetValue<int>("RunnerSettings:PublisherLimit");

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                unitOfWork.Begin();

                var reportRequests = await _reportBusiness.GetReportQueryByStatus(unitOfWork, ReportStatusEnum.Pending, limit);

                foreach (var request in reportRequests)
                {
                    try
                    {
                        _logger.LogInformation($"{request.Id} publishing.");
                        _publisherService.Enqueue<Report>(request);
                        request.ReportStatus = ReportStatusEnum.Processing;
                        await _reportBusiness.Update(unitOfWork, request);
                        _logger.LogInformation($"{request.Id} published.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Name : {nameof(ReportPublisherWorker)} Id: {request.Id} publish fail.");
                    }
                    finally
                    {
                        await _reportBusiness.Update(unitOfWork, request);
                        unitOfWork.Commit();
                    }
                }
            }

        }
    }
}