using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Common.Enums;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;

namespace YellowPages.Business.Concrete
{
    public class ReportBusiness : IReportBusiness
    {
        private readonly IReportRepository reportRepository;
        public ReportBusiness(IReportRepository _reportRepository)
        {
            reportRepository = _reportRepository;
        }

        public async Task<bool> DeleteById(IUnitOfWork unitOfWork, Guid id)
        {
            try
            {
                var entity = await reportRepository.GetByIdAsync(unitOfWork, id);
                await reportRepository.DeleteAsync(unitOfWork, entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<Report>> GetReportQuery(IUnitOfWork unitOfWork) => await reportRepository.GetListAsync(unitOfWork);
        public async Task<Report> GetById(IUnitOfWork unitOfWork, Guid id) => await reportRepository.GetByIdAsync(unitOfWork, id);
        public async Task<Guid> Insert(IUnitOfWork unitOfWork, Report report)
        {
            await reportRepository.InsertAsync(unitOfWork, report);
            return report.Id;
        }
        public async Task<bool> Update(IUnitOfWork unitOfWork, Report report)
        {
            try
            {
                await reportRepository.UpdateAsync(unitOfWork, report);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<Report>> GetReportQueryByStatus(IUnitOfWork unitOfWork, ReportStatusEnum reportStatus, int limit) => await reportRepository.GetReportQueryByStatus(unitOfWork, reportStatus, limit);
    }
}
