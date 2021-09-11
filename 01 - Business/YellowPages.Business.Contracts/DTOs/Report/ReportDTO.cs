using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;

namespace YellowPages.Business.Contracts.DTOs.Report
{
    public class ReportDTO : DTOBase<Guid>
    {
        public ReportStatusEnum ReportStatus { get; set; }
    }
}
