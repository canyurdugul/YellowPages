using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Business.Contracts.DTOs.Person
{
    public class PersonDTO : DTOBase<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FirmName { get; set; }
        public string FullName { get; set; }
        public List<PersonContactInfoDTO> PersonContactInfos { get; set; }
    }
}
