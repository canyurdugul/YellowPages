using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Business.Concrete;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Business.Contracts.Validations;
using YellowPages.Database.Data;
using YellowPages.Database.Data.Factory;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.Repository.Concrete;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Database.UnitOfWork.Concrete;

namespace YellowPages.Client.ApiPerson
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
            services.AddTransient<IPersonBusiness, PersonBusiness>();
            services.AddTransient<IPersonRepository, PersonRepository>(); 
            services.AddTransient<IPersonContactInfoRepository, PersonContactInfoRepository>();

            services.AddTransient<IValidator<PersonDTO>, PersonValidation>();
            services.AddTransient<IValidator<PersonContactInfoDTO>, PersonContactInfoValidation>();

            services.AddSingleton<IDbContextFactory, DbContextFactory>();
            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

            using (var context = new YellowPagesContext())
            {
                context.Database.Migrate();
            }
        }

    }
}
