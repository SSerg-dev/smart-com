using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(BTLPromo))]
    public class HistoricalBTLPromo
    {
        public Guid BTLId { get; set; }
        public Guid PromoId { get; set; }
    }
}
