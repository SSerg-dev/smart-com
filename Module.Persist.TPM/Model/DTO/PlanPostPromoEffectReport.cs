using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO {
    public class PlanPostPromoEffectReport : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ZREP { get; set; }
        public string DemandCode { get; set; }
        public string PromoNameId { get; set; }
        public int PromoNumber { get; set; }
        public string LocApollo { get; set; }
        public string TypeApollo { get; set; }
        public string ModelApollo { get; set; }
        public DateTimeOffset? WeekStartDate { get; set; }
        public double? PlanPostPromoEffectQty { get; set; }
		public double? PlanUplift { get; set; }
		public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
		public string Week { get; set; }
		public string Status { get; set; }
		public double? PlanProductBaselineCaseQty { get; set; }
		public double? PlanProductPostPromoEffectLSV { get; set; }
		public double? PlanProductBaselineLSV { get; set; }
		public bool? InOut { get; set; }
        public bool IsOnInvoice { get; set; }
    }
}
