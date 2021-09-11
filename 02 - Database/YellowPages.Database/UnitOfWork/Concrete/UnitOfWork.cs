using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Data.Factory;
using YellowPages.Database.UnitOfWork.Abstracts;

namespace YellowPages.Database.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DbContextModel dbContextModel;
        private readonly string httpContextKey = "UnitOfWorkStack";
        private DbContext dbContext;
        private IDbContextTransaction dbContextTransaction;

        public UnitOfWork(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        public UnitOfWork(IHttpContextAccessor httpContextAccessor, DbContextModel dbContextModel, bool isUsedStack)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContextModel = dbContextModel;
            this.DbContextPrepare(dbContextModel, isUsedStack);
        }

        private void DbContextPrepare(DbContextModel dbContextModel, bool isUsedStack)
        {
            if (!isUsedStack)
            {
                this.dbContext = (DbContext)dbContextModel.DbContext;
            }
            else
            {
                Stack<object> objectStack;
                if (this.httpContextAccessor.HttpContext.Items.ContainsKey((object)this.httpContextKey))
                {
                    objectStack = this.httpContextAccessor.HttpContext.Items[(object)this.httpContextKey] as Stack<object>;
                }
                else
                {
                    objectStack = new Stack<object>();
                    this.httpContextAccessor.HttpContext.Items[(object)this.httpContextKey] = (object)objectStack;
                }
                objectStack.Push(dbContextModel.DbContext);
            }
        }

        public TDbContext GetCurrentDbContext<TDbContext>() => (TDbContext)this.dbContextModel.DbContext;

        internal TDbContext GetDbContextFromStack<TDbContext>(bool isDispose)
        {
            Stack<object> objectStack = this.httpContextAccessor.HttpContext.Items[(object)this.httpContextKey] as Stack<object>;
            return !isDispose ? (TDbContext)objectStack.Peek() : (TDbContext)objectStack.Pop();
        }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
                this.dbContextTransaction?.Dispose();
            }
            else
                this.GetDbContextFromStack<DbContext>(true).Dispose();
        }
        public void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) => this.dbContextTransaction = this.dbContext.Database.BeginTransaction();

        public void Commit()
        {
            if (this.dbContext != null)
            {
                this.dbContext.SaveChanges();
                this.dbContextTransaction?.Commit();
            }
            else
                this.GetDbContextFromStack<DbContext>(false).SaveChanges();
        }
    }
}
