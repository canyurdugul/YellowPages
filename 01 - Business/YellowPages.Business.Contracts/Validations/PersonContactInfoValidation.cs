using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Common.Validation;

namespace YellowPages.Business.Contracts.Validations
{
    public class PersonContactInfoValidation : Validator<PersonContactInfoDTO>
    {
        public PersonContactInfoValidation()
        {
            ValidateEMail();
            ValidatePhoneNumber();
            ValidateLocation();
            ValidateContactType();
        }

        public void ValidatePhoneNumber()
        {
            When(w => w.ContactType == Common.Enums.ContactTypeEnum.PhoneNumber, () =>
            {
                var phoneNumberRegex = new Regex(@"(05|5)[0-9][0-9][1-9]([0-9]){6}");
                RuleFor(x => x.ContactInfo).Must((x, item, context) =>
                {
                    var isValid = false;
                    if (phoneNumberRegex.IsMatch(x.ContactInfo))
                        isValid = true;
                    else
                        isValid = false;
                    return isValid;
                }).WithMessage("Please enter valid Turkey phone number.");

            });
        }
        public void ValidateEMail()
        {
            When(w => w.ContactType == Common.Enums.ContactTypeEnum.EMailAddress, () =>
            {
                
                RuleFor(x => x.ContactInfo).Must((x, item, context) =>
                {
                    return new MailAddress(x.ContactInfo).Address == x.ContactInfo;
                }).WithMessage("Please enter valid email.");
            });
        }
        public void ValidateLocation()
        {
            When(w => w.ContactType == Common.Enums.ContactTypeEnum.Location, () =>
            {
                RuleFor(x => x.ContactInfo)
                .NotEmpty()
                .NotNull()
                .Length(1, 30)
                .WithMessage("Please enter valid location.");
            });
        }
        public void ValidateContactType()
        {
            RuleFor(x => x.ContactType)
                .Must((x, item, context) =>
                {
                    var isValid = false;
                    if ((int)x.ContactType > 0 && (int)x.ContactType < 4)
                        isValid = true;
                    else
                        isValid = false;
                    return isValid;
                }).WithMessage("Please enter valid contact type");
        }
    }
}
