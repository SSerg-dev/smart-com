using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Enum
{    
    public static class RSstateNames
    {
        public const string DRAFT = "Draft";
        public const string ON_APPROVAL = "On Approval";
        public const string APPROVED = "Approved";
        public const string CANCELLED = "Cancelled";
        public const string WAITING = "Waiting";
        public const string CALCULATING = "Calculating";
    }
}
