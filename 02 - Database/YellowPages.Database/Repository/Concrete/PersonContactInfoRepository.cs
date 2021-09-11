using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Data;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Database.Repository.Concrete
{
    public class PersonContactInfoRepository : Repository<PersonContactInfo, Guid>, IPersonContactInfoRepository
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public PersonContactInfoRepository(IUnitOfWorkFactory _unitOfWorkFactory) : base(_unitOfWorkFactory)
        {
            this.unitOfWorkFactory = _unitOfWorkFactory;
        }

        public async Task<IEnumerable<PersonContactInfo>> GetPersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId)
        {
            var context = unitOfWork.GetCurrentDbContext<YellowPagesContext>();
            return (IEnumerable<PersonContactInfo>)await context.PersonContactInfos.Where(w => w.PersonId == personId).ToListAsync();
        }
    }
}
