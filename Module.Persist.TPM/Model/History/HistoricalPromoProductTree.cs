using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoProductTree))]
    public class HistoricalPromoProductTree : BaseHistoricalEntity<System.Guid>
    {
        public Guid PromoId { get; set; }        
        public int ProductTreeObjectId { get; set; }
    }
}
