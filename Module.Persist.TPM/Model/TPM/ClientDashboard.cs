using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ClientDashboard : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public int? ClientTreeId { get; set; }
        public string ClientHierarchy { get; set; }
        public Guid? BrandTechId { get; set; }
        public string BrandsegTechsubName { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        public double? ShopperTiPlanPercent { get; set; }
        public double? MarketingTiPlanPercent { get; set; }
        public double? ProductionPlan { get; set; }
        public double? BrandingPlanPercent { get; set; }
        public double? BrandingPlan { get; set; }
        public double? BTLPlan { get; set; }
        public double? ROIPlanPercent { get; set; }
        public double? IncrementalNSVPlan { get; set; }
        public double? PromoNSVPlan { get; set; }
        public double? PlanLSV { get; set; }
        public double? PromoTiCostPlanPercent { get; set; }
        public double? NonPromoTiCostPlanPercent { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
