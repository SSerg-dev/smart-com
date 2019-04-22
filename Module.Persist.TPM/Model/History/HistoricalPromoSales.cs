using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(PromoSales))]
    public class HistoricalPromoSales : BaseHistoricalEntity<System.Guid>
    {
        public Guid? ClientId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? BrandTechId { get; set; }
        public Guid? PromoStatusId { get; set; }
        public Guid? MechanicId { get; set; }
        public Guid? BudgetItemId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public string ClientCommercialSubnetCommercialNetName { get; set; }
        public string BrandName { get; set; }
        public string BrandTechName { get; set; }
        public string PromoStatusName { get; set; }
        public string MechanicMechanicName { get; set; }
        public int? MechanicDiscount { get; set; }
        public string MechanicComment { get; set; }
        public int? Plan { get; set; }
        public int? Fact { get; set; }
        public string BudgetItemBudgetName { get; set; }
        public string BudgetItemName { get; set; }
    }
}
