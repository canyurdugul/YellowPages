using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;

namespace YellowPages.Business.Abstracts
{
    public interface IReportBusiness
    {
        Task<IEnumerable<Report>> GetReportQuery(IUnitOfWork unitOfWork);
        Task<Report> GetById(IUnitOfWork unitOfWork, Guid id);
        Task<Guid> Insert(IUnitOfWork unitOfWork, Report person);
        Task<bool> DeleteById(IUnitOfWork unitOfWork, Guid value);
        Task<bool> Update(IUnitOfWork unitOfWork, Report report);
        Task<IEnumerable<Report>> GetReportQueryByStatus(IUnitOfWork unitOfWork, ReportStatusEnum reportStatus, int limit);
    }
}
