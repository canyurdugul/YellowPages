using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Database.Repository.Concrete
{
    public class PersonRepository : Repository<Person, Guid>, IPersonRepository
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public PersonRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}
