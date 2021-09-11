using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;
using YellowPages.Database.Data;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Report;

namespace YellowPages.Database.Repository.Concrete
{
    public class ReportRepository : Repository<Report, Guid>, IReportRepository
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ReportRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IEnumerable<Report>> GetReportQueryByStatus(IUnitOfWork unitOfWork, ReportStatusEnum reportStatus, int limit)
        {
            var context = unitOfWork.GetCurrentDbContext<YellowPagesContext>();
            return (IEnumerable<Report>)await context.Reports.Where(w => w.ReportStatus == reportStatus).OrderBy(o => o.CreatedDate).Take(limit).ToListAsync();
        }
    }
}
