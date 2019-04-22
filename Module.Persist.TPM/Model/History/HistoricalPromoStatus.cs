using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History {
    [AssociatedWith(typeof(PromoStatus))]
    public class HistoricalPromoStatus : BaseHistoricalEntity<System.Guid> {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public string Color { get; set; }

    }
}
