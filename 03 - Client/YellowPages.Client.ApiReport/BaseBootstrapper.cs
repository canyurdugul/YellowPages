using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Business.Concrete;
using YellowPages.Database.Data;
using YellowPages.Database.Data.Factory;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.Repository.Concrete;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Database.UnitOfWork.Concrete;

namespace YellowPages.Client.ApiReport
{
    public class BaseBootstrapper
    {
        private readonly IServiceCollection services;

        public BaseBootstrapper(IServiceCollection services)
        {
            this.services = services;
            Install();
        }

        private void Install()
        {
            services.AddTransient<IReportBusiness, ReportBusiness>();
            services.AddTransient<IReportRepository, ReportRepository>();


            services.AddTransient<IDbContextFactory, DbContextFactory>();
            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

            using (var context = new YellowPagesContext())
            {
                context.Database.Migrate();
            }
        }

    }
}
