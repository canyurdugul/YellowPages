using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;

namespace YellowPages.Database.Repository.Abstracts
{
    public interface IReportRepository : IRepository<Report, Guid>
    {
        Task<IEnumerable<Report>> GetReportQueryByStatus(IUnitOfWork unitOfWork, ReportStatusEnum reportStatus, int limit);
    }
}
