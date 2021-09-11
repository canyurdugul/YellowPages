using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Entities
{
    public class AuditLog : ModelBase<Guid>
    {
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        public string OldEntityValue { get; set; } // OldEntityValue
        public string NewEntityValue { get; set; } // NewEntityValue
        public int Type { get; set; } // Type
        public string Text { get; set; } // Text
        public DateTime? LogDate { get; set; }
    }
}
