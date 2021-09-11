using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Entities.Person
{
    public class Person : ModelBase<Guid>
    { 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FirmName { get; set; }
        public string FullName { get; set; }
        public virtual List<PersonContactInfo> PersonContactInfos { get; set; }
    }
}
