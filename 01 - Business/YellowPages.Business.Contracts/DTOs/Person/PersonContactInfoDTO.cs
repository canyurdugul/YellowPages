using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Common.Enums;

namespace YellowPages.Business.Contracts.DTOs.Person
{
    public class PersonContactInfoDTO : DTOBase<Guid>
    {
        public Guid PersonId { get; set; }
        public ContactTypeEnum ContactType { get; set; }
        public string ContactInfo { get; set; }
    }
}
