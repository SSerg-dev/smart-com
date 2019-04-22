using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoDemand))]
    public class HistoricalPromoDemand : BaseHistoricalEntity<System.Guid>
    {
        public Guid BrandTechId { get; set; }
        public Guid? MechanicId { get; set; }
        public Guid? MechanicTypeId { get; set; }

        public string BrandTechBrandName { get; set; }
        public string BrandTechName { get; set; }
        public string Account { get; set; }
        public string MechanicName { get; set; }
        public string MechanicTypeName { get; set; }
        public int Discount { get; set; }
        public string Week { get; set; }
        public DateTimeOffset MarsStartDate { get; set; }
        public DateTimeOffset MarsEndDate { get; set; }
        public double Baseline { get; set; }
        public double Uplift { get; set; }
        public double Incremental { get; set; }
        public double Activity { get; set; }

    }
}
