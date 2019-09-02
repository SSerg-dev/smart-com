using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(IncrementalPromo))]
    public class HistoricalIncrementalPromo : BaseHistoricalEntity<System.Guid>
    {
        public Guid Id { get; set; }
        public Guid PromoId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductZREP { get; set; }
        public string ProductProductEN { get; set; }
        public string PromoClientHierarchy { get; set; }
        public int PromoNumber { get; set; }
        public string PromoName { get; set; }

        public double? PlanPromoIncrementalCases { get; set; }
        public double? CasePrice { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
