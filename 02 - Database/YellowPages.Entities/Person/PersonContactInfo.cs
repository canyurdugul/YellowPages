using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YellowPages.Common.Enums;

namespace YellowPages.Entities.Person
{
    public class PersonContactInfo : ModelBase<Guid>
    {
        public Guid PersonId { get; set; }
        public ContactTypeEnum ContactType { get; set; }
        public string ContactInfo { get; set; }

        #region FK
        [JsonIgnore]
        public virtual Person Person { get; set; }
        #endregion
    }
}
