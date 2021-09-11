using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Data.Factory;
using YellowPages.Database.UnitOfWork.Abstracts;

namespace YellowPages.Database.UnitOfWork.Concrete
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDbContextFactory dbContextFactory;

        public UnitOfWorkFactory(
          IHttpContextAccessor httpContextAccessor,
          IDbContextFactory dbContextFactory)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContextFactory = dbContextFactory;
        }

        public IUnitOfWork Create() => (IUnitOfWork)new UnitOfWork(this.httpContextAccessor, this.dbContextFactory.GetDbContextModel(), false);

        public IUnitOfWork CreateWithStack() => (IUnitOfWork)new UnitOfWork(this.httpContextAccessor, this.dbContextFactory.GetDbContextModel(), true);

        public TDbContext GetDbContextFromStack<TDbContext>() => new UnitOfWork(this.httpContextAccessor).GetDbContextFromStack<TDbContext>(false);
    }
}
