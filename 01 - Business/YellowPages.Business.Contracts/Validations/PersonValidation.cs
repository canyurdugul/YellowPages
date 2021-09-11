using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Common.Validation;

namespace YellowPages.Business.Contracts.Validations
{
    public class PersonValidation : Validator<PersonDTO>
    {
        public PersonValidation()
        {
            ValidateName();
            ValidateSurname();
            ValidateFirmName(); 
        }

        public void ValidateName()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .Length(1, 100)
                .WithMessage("Please enter valid person name");
        }
        public void ValidateSurname()
        {
            RuleFor(x =>x.Surname)
                .NotEmpty()
                .NotNull()
                .Length(1, 500)
                .WithMessage("Please enter valid person surname");
        }
        public void ValidateFirmName()
        {
            RuleFor(x => x.FirmName)
                .NotEmpty()
                .NotNull()
                .Length(1, 200)
                .WithMessage("Please enter valid firm name");
        }
    }
}
