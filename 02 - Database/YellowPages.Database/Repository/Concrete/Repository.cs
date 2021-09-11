using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities;

namespace YellowPages.Database.Repository.Concrete
{
    public class Repository<TEntity, PKey> : IRepository<TEntity, PKey> where TEntity : ModelBase<PKey>
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        public Repository(IUnitOfWorkFactory _unitOfWorkFactory) => this.unitOfWorkFactory = _unitOfWorkFactory;

        #region Crud Operations
        public async Task<TEntity> GetByIdAsync(IUnitOfWork unitOfWork, PKey Id)
        {
            return await unitOfWork.GetCurrentDbContext<DbContext>().Set<TEntity>().Where<TEntity>((Expression<Func<TEntity, bool>>)(x => x.Id.Equals((object)Id))).FirstOrDefaultAsync<TEntity>();
        }
        public async Task<TEntity> GetAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate)
        {
            return await unitOfWork.GetCurrentDbContext<DbContext>().Set<TEntity>().Where<TEntity>(predicate).FirstOrDefaultAsync<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetListAsync(IUnitOfWork unitOfWork)
        {
            return (IEnumerable<TEntity>)await unitOfWork.GetCurrentDbContext<DbContext>().Set<TEntity>().ToListAsync<TEntity>();
        }
        public async Task InsertAsync(IUnitOfWork unitOfWork, TEntity entity)
        {
            DbContext currentDbContext = unitOfWork.GetCurrentDbContext<DbContext>();
            EntityEntry<TEntity> entityEntry = await currentDbContext.Set<TEntity>().AddAsync(entity);
            var num = await currentDbContext.SaveChangesAsync();
            currentDbContext = (DbContext)null;
        }
        public async Task UpdateAsync(IUnitOfWork unitOfWork, TEntity entity)
        {
            DbContext currentDbContext = unitOfWork.GetCurrentDbContext<DbContext>();
            currentDbContext.Entry<TEntity>(entity).State = EntityState.Modified;

            #region Audit
            var entries = currentDbContext.ChangeTracker.Entries();
            var logRecords = GetLogs(currentDbContext, entries);
            if (logRecords.Count > 0)
                await SaveAuditLogsAsync(currentDbContext, logRecords);
            #endregion

            int num = await currentDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(IUnitOfWork unitOfWork, TEntity entity)
        {
            DbContext currentDbContext = unitOfWork.GetCurrentDbContext<DbContext>();
            currentDbContext.Entry<TEntity>(entity).State = EntityState.Deleted;

            #region Audit
            var entries = currentDbContext.ChangeTracker.Entries();
            var logRecords = GetLogs(currentDbContext, entries);
            if (logRecords.Count > 0)
                await SaveAuditLogsAsync(currentDbContext, logRecords);
            #endregion

            int num = await currentDbContext.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(IUnitOfWork unitOfWork, TEntity entity)
        {
            DbContext currentDbContext = unitOfWork.GetCurrentDbContext<DbContext>();
            ((object)entity as ModelBase<PKey>).IsDeleted = true;
            currentDbContext.Entry<TEntity>(entity).State = EntityState.Modified;

            #region Audit
            var entries = currentDbContext.ChangeTracker.Entries();
            var logRecords = GetLogs(currentDbContext, entries);
            if (logRecords.Count > 0)
                await SaveAuditLogsAsync(currentDbContext, logRecords);
            #endregion

            int num = await currentDbContext.SaveChangesAsync();
        }
        #endregion

        #region Audit Operations
        public List<AuditLog> GetLogs(DbContext dbContext, IEnumerable<EntityEntry> entries)
        {
            List<AuditLog> logs = new List<AuditLog>();
            if (dbContext != null)
            {
                entries = entries.Where(w => w.State == EntityState.Modified || w.State == EntityState.Deleted);
                if (entries.Count() == 0)
                    return logs;

                logs = entries.Select(entry =>
                       {
                           Type type = entry.Entity.GetType();
                           var log = new AuditLog();
                           log.EntityName = type.Name;
                           log.EntityId = (entry.Entity as ModelBase<Guid>).Id;
                           if (entry.State == EntityState.Deleted)
                           {
                               var oldVals = entry.GetDatabaseValues().ToObject();
                               log.OldEntityValue = Newtonsoft.Json.JsonConvert.SerializeObject(oldVals);
                               log.Type = (byte)EntityState.Deleted;
                               log.Text = "Record deleted.";
                           }
                           else if (entry.State == EntityState.Modified)
                           {
                               var oldVals = entry.GetDatabaseValues()?.ToObject();
                               log.OldEntityValue = Newtonsoft.Json.JsonConvert.SerializeObject(oldVals);
                               var newVals = entry.CurrentValues.ToObject();
                               log.NewEntityValue = Newtonsoft.Json.JsonConvert.SerializeObject(newVals);
                               log.Type = (byte)EntityState.Modified;
                               log.Text = "Record updated.";

                           }
                           log.LogDate = DateTime.Now;
                           return log;
                       })
                       .ToList();
            }
            return logs;
        }
        public async Task SaveAuditLogsAsync(DbContext dbContext, List<AuditLog> logs)
        {
            if (dbContext != null && logs != null && logs.Count > 0)
                await dbContext.Set<AuditLog>().AddRangeAsync(logs);
        }
        #endregion
    }
}
