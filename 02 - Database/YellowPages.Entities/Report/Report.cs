using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;

namespace YellowPages.Entities.Report
{
    public class Report : ModelBase<Guid>
    {
        public ReportStatusEnum ReportStatus { get; set; }
        public string ReportPath { get; set; }
    }
}
