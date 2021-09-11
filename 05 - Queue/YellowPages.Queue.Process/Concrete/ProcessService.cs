using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Data;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Queue.Common;
using YellowPages.Queue.Process.Abstract;


namespace YellowPages.Queue.Process.Concrete
{
    // Rapor burada olusacak.
    public sealed class ProcessService : IProcessService
    {
        private readonly ILogger<ProcessService> _logger;
        public ProcessService(ILogger<ProcessService> logger)
        {
            _logger = logger;
        }

        public async Task<string> Execute(IUnitOfWork unitOfWork)
        {
            try
            {
                _logger.LogInformation($"Processing at ProcessService");
                var context = unitOfWork.GetCurrentDbContext<YellowPagesContext>();
                var data = await context.Persons.Include(i => i.PersonContactInfos).ToListAsync();
                var reportData = new List<ReportModel>();
                foreach (var item in data)
                {
                    if (item.PersonContactInfos != null && item.PersonContactInfos.Count > 0)
                    {
                        foreach (var info in item.PersonContactInfos)
                        {
                            reportData.Add(new ReportModel
                            {
                                FullName = item.FullName,
                                FirmName = item.FirmName,
                                ContactInfo = info.ContactInfo,
                                ContactType = info.ContactType.ToString()
                            });
                        }
                    }
                    else
                    {
                        reportData.Add(new ReportModel
                        {
                            FirmName = item.FirmName,
                            FullName = item.FullName,
                            ContactType = "",
                            ContactInfo = ""
                        });
                    }
                }
                var reportByteArray = ReportHelper.ReportHelper.GetFileReport(reportData);
                var fileName = $"Reports\\{Guid.NewGuid()}.xlsx";
                var fullPath = System.IO.Path.GetFullPath($"Reports\\{Guid.NewGuid()}.xlsx");
                System.IO.File.WriteAllBytes(fileName, reportByteArray);
                _logger.LogInformation($"Processed at ProcessService. File saved. FileName : {fileName}");

                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex}");
                return null;
            }

        }
    }
}
