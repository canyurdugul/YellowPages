using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Database.Data;
using YellowPages.Database.Repository.Abstracts;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Business.Concrete
{
    public class PersonBusiness : IPersonBusiness
    {
        private readonly IPersonRepository personRepository;
        private readonly IPersonContactInfoRepository personContactInfoRepository;
        public PersonBusiness(IPersonRepository _personRepository, IPersonContactInfoRepository _personContactInfoRepository)
        {
            personRepository = _personRepository;
            personContactInfoRepository = _personContactInfoRepository;
        }

        #region Person
        public async Task<IEnumerable<Person>> GetPersonQuery(IUnitOfWork unitOfWork) => await personRepository.GetListAsync(unitOfWork);
        public async Task<Person> GetById(IUnitOfWork unitOfWork, Guid id)
        {
            var context = unitOfWork.GetCurrentDbContext<YellowPagesContext>();
            return await context.Persons.Include(i => i.PersonContactInfos).FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<Guid> Insert(IUnitOfWork unitOfWork, Person person)
        {
            await personRepository.InsertAsync(unitOfWork, person);
            return person.Id;
        }
        public async Task<bool> DeleteById(IUnitOfWork unitOfWork, Guid id)
        {
            try
            {
                var entity = await personRepository.GetByIdAsync(unitOfWork, id);
                await personRepository.DeleteAsync(unitOfWork, entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> Update(IUnitOfWork unitOfWork, Person person)
        {
            try
            {
                await personRepository.UpdateAsync(unitOfWork, person); 
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Person Contact Info
        public async Task<IEnumerable<PersonContactInfo>> GetPersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId) => await personContactInfoRepository.GetPersonContactInfoByPersonId(unitOfWork, personId);
        public async Task<PersonContactInfo> GetPersonContactInfoById(IUnitOfWork unitOfWork, Guid id) => await personContactInfoRepository.GetByIdAsync(unitOfWork, id);
        public async Task<Guid> InsertPersonContactInfo(IUnitOfWork unitOfWork, PersonContactInfo personContactInfo)
        {
            await personContactInfoRepository.InsertAsync(unitOfWork, personContactInfo);
            return personContactInfo.Id;
        }
        public async Task<bool> DeletePersonContactInfoById(IUnitOfWork unitOfWork, Guid id)
        {
            try
            {
                var entity = await personContactInfoRepository.GetByIdAsync(unitOfWork, id);
                await personContactInfoRepository.DeleteAsync(unitOfWork, entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeletePersonContactInfoByPersonId(IUnitOfWork unitOfWork, Guid personId)
        {
            try
            {
                var entities = await personContactInfoRepository.GetPersonContactInfoByPersonId(unitOfWork, personId);
                foreach (var entity in entities)
                {
                    await personContactInfoRepository.DeleteAsync(unitOfWork, entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdatePersonContactInfo(IUnitOfWork unitOfWork, PersonContactInfo personContactInfo)
        {
            try
            {
                await personContactInfoRepository.UpdateAsync(unitOfWork, personContactInfo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


    }
}
