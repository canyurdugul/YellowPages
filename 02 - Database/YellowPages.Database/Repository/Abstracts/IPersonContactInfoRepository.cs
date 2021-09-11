using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Database.Repository.Abstracts
{
    public interface IPersonContactInfoRepository : IRepository<PersonContactInfo, Guid>
    {
        Task<IEnumerable<PersonContactInfo>> GetPersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId);
    }
}
