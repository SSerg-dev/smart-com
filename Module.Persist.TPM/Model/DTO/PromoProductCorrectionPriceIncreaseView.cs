using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Persist.TPM.Model.DTO
{
    public class PromoProductCorrectionPriceIncreaseView : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string ClientHierarchy { get; set; }
        public string BrandTechName { get; set; }
        public string ProductSubrangesList { get; set; }
        public string MarsMechanicName { get; set; }
        public string EventName { get; set; }
        public string PromoStatusSystemName { get; set; }
        public string MarsStartDate { get; set; }
        public string MarsEndDate { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
        public double? PlanProductIncrementalLSV { get; set; }
        public double? PlanProductLSV { get; set; }
        public string ZREP { get; set; }
        public double? PlanProductUpliftPercentCorrected { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }
        public string UserName { get; set; }
        public int? ClientTreeId { get; set; }
        public bool Disabled { get; set; }
        public Guid? PromoProductId { get; set; }
        public Guid? UserId { get; set; }

        public DateTimeOffset? PromoDispatchStartDate { get; set; }
        public string PromoStatusName { get; set; }

        public bool IsGrowthAcceleration { get; set; }
		public bool IsInExchange { get; set; }

    }
}
