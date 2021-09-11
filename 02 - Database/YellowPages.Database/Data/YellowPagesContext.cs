using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Entities;
using YellowPages.Entities.Person;
using YellowPages.Entities.Report;

namespace YellowPages.Database.Data
{
    public class YellowPagesContext : DbContext
    {
        public YellowPagesContext()
        {

        }

        #region DbSets
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonContactInfo> PersonContactInfos { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        #endregion

        #region Life Cycle
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string appSettings = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            appSettings = appSettings ?? "Development";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.{appSettings}.json")
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
        #endregion
    }
}
