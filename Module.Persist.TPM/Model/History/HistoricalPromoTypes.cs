using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoTypes))]
    public class HistoricalPromoTypes : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
    }
}
