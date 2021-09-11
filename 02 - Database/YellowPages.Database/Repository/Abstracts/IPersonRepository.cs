using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Entities.Person;

namespace YellowPages.Database.Repository.Abstracts
{
    public interface IPersonRepository : IRepository<Person, Guid>
    {
        
    }
}
