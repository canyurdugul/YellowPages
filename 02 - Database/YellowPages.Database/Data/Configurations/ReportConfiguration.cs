using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Entities.Report;

namespace YellowPages.Database.Data.Configurations
{
    class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");
            builder.HasQueryFilter(f => f.IsDeleted == false);
            builder.Property(s => s.ReportStatus);
            builder.Property(s => s.ReportPath);
            builder.Property(s => s.IsDeleted).HasDefaultValue(false);
        }
    }
}
