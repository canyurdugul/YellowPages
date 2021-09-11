using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities;

namespace YellowPages.Database.Repository.Abstracts
{
    public interface IRepository<TEntity, PKey> where TEntity : ModelBase<PKey>
    {
        Task<TEntity> GetByIdAsync(IUnitOfWork unitOfWork, PKey Id);
        Task<TEntity> GetAsync(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListAsync(IUnitOfWork unitOfWork);

        Task InsertAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task UpdateAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task DeleteAsync(IUnitOfWork unitOfWork, TEntity entity);
        Task SoftDeleteAsync(IUnitOfWork unitOfWork, TEntity entity);
    }
}
