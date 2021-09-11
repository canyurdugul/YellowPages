using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Business.Abstracts
{
    public interface IPersonBusiness
    {
        Task<IEnumerable<Person>> GetPersonQuery(IUnitOfWork unitOfWork);
        Task<Person> GetById(IUnitOfWork unitOfWork, Guid id);
        Task<Guid> Insert(IUnitOfWork unitOfWork, Person person);
        Task<bool> DeleteById(IUnitOfWork unitOfWork, Guid value);
        Task<bool> Update(IUnitOfWork unitOfWork, Person person);

        #region Contact Infos
        Task<IEnumerable<PersonContactInfo>> GetPersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId);
        Task<PersonContactInfo> GetPersonContactInfoById(IUnitOfWork unitOfWork, Guid id);
        Task<Guid> InsertPersonContactInfo(IUnitOfWork unitOfWork, PersonContactInfo personContactInfo);
        Task<bool> DeletePersonContactInfoById(IUnitOfWork unitOfWork, Guid id);
        Task<bool> DeletePersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId);
        Task<bool> UpdatePersonContactInfo(IUnitOfWork unitOfWork, PersonContactInfo personContactInfo);
        #endregion
    }
}
