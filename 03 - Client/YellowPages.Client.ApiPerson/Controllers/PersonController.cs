using AutoWrapper.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;

namespace YellowPages.Client.ApiPerson.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IPersonBusiness _personBusiness;
        private readonly IValidator<PersonDTO> _personValidator;
        private readonly IValidator<PersonContactInfoDTO> _personContactInfoValidator;

        public PersonController(ILogger<PersonController> logger, IUnitOfWorkFactory unitOfWorkFactory, IPersonBusiness personBusiness, IValidator<PersonDTO> personValidator, IValidator<PersonContactInfoDTO> personContactInfoValidator)
        {
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _personBusiness = personBusiness;
            _personValidator = personValidator;
            _personContactInfoValidator = personContactInfoValidator;
        }

        #region Person Operations
        [HttpGet]
        [Route("get/{id:guid}")]
        public async Task<JsonResult> Get(Guid? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.GetById(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        [HttpGet]
        [Route("get-all")]
        public async Task<JsonResult> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.GetPersonQuery(unitOfWork);
                return new JsonResult(result.Select(s => new PersonDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    FirmName = s.FirmName,
                    CreatedDate = s.CreatedDate,
                    PersonContactInfos = s.PersonContactInfos != null ? s.PersonContactInfos.Select(spci => new PersonContactInfoDTO
                    {
                        Id = spci.Id,
                        ContactInfo = spci.ContactInfo,
                        ContactType = spci.ContactType
                    }).ToList() : new List<PersonContactInfoDTO>()
                }));
            }
        }
        [HttpPost]
        [Route("create-or-update")]
        public async Task<JsonResult> CreateOrUpdate(PersonDTO personDTO)
        {
            var validationResult = _personValidator.Validate(personDTO);
            if (!validationResult.IsValid)
                return new JsonResult(BadRequest(validationResult.Errors.Select(s => s.ErrorMessage)));

            var person = new Person
            {
                Name = personDTO.Name,
                Surname = personDTO.Surname,
                FirmName = personDTO.FirmName,
                Id = personDTO.Id,
                FullName = string.Format("{0} {1}", personDTO.Name, personDTO.Surname),
                PersonContactInfos = personDTO.PersonContactInfos != null && personDTO.PersonContactInfos.Count > 0 ? personDTO.PersonContactInfos.Select(s => new PersonContactInfo { ContactInfo = s.ContactInfo, ContactType = s.ContactType, Id = s.Id, PersonId = s.PersonId }).ToList() : null
            };

            var unitOfWork = _unitOfWorkFactory.Create();
            if (person.Id != Guid.Empty)
            {
                var result = await _personBusiness.Update(unitOfWork, person); 
                return new JsonResult(result);
            }
            else
            {
                var result = await _personBusiness.Insert(unitOfWork, person); 
                return new JsonResult(result);
            }

        }
        [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<JsonResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return new JsonResult(BadRequest("Id parameter can't be null"));

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.DeleteById(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        #endregion

        #region Person Contact Info Operations
        [HttpGet]
        [Route("get-person-contact-info-by-person-id/{id:guid}")]
        public async Task<JsonResult> GetPersonContactInfoByPersonId(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return new JsonResult(Newtonsoft.Json.JsonConvert.SerializeObject(new PersonContactInfoDTO()));
            //return new JsonResult(BadRequest("Id parameter can't be null"));

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.GetPersonContactInfoByPersonId(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        [HttpPost]
        [Route("person-contact-info-create-or-update")]
        public async Task<JsonResult> PersonContactInfoCreateOrUpdate(PersonContactInfoDTO personContactInfoDTO)
        {
            var validationResult = _personContactInfoValidator.Validate(personContactInfoDTO);
            if (!validationResult.IsValid)
                return new JsonResult(BadRequest(validationResult.Errors.Select(s => s.ErrorMessage)));

            var personContactInfo = new PersonContactInfo
            {
                ContactInfo = personContactInfoDTO.ContactInfo,
                ContactType = personContactInfoDTO.ContactType,
                Id = personContactInfoDTO.Id,
                PersonId = personContactInfoDTO.PersonId
            };

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                if (personContactInfo.Id != Guid.Empty)
                {
                    var result = await _personBusiness.UpdatePersonContactInfo(unitOfWork, personContactInfo);
                    return new JsonResult(result);
                }
                else
                {
                    var result = await _personBusiness.InsertPersonContactInfo(unitOfWork, personContactInfo);
                    if (result != Guid.Empty)
                        return new JsonResult(true);
                    else
                        return new JsonResult(false);
                }
            }
        }
        [HttpGet]
        [Route("delete-person-contact-info-by-id/{id:guid}")]
        public async Task<JsonResult> DeletePersonContactInfoById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return new JsonResult(BadRequest("Id parameter can't be null"));

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.DeletePersonContactInfoById(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        [HttpGet]
        [Route("delete-person-contact-info-by-person-id/{id:guid}")]
        public async Task<JsonResult> DeletePersonContactInfoByPersonId(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return new JsonResult(BadRequest("Id parameter can't be null"));

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _personBusiness.DeletePersonContactInfoByPersonId(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        #endregion
    }
}
