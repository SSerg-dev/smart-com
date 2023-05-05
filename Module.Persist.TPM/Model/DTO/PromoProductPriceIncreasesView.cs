using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO
{
    [Table("PromoProductPriceIncreasesView")]
    public class PromoProductPriceIncreasesView : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(255)]
        public string ZREP { get; set; }
        public string ProductEN { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
        public double? PlanProductUpliftPercent { get; set; }
        public double? PlanProductIncrementalLSV { get; set; }
        public double? PlanProductLSV { get; set; }
        public double? PlanProductBaselineCaseQty { get; set; }
        public double? PlanProductIncrementalCaseQty { get; set; }
        public double? PlanProductCaseQty { get; set; }
        public bool AverageMarker { get; set; }
        public bool IsCorrection { get; set; }
    }
}
