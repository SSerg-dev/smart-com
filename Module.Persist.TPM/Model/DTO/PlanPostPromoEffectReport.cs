using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO {
    public class PlanPostPromoEffectReport : IEntity<Guid> {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ZREP { get; set; }
        public string DemandCode { get; set; }
        public string PromoName { get; set; }
        public string PromoNameId { get; set; }
        public string LocApollo { get; set; }
        public string TypeApollo { get; set; }
        public string ModelApollo { get; set; }
        public DateTimeOffset? WeekStartDate { get; set; }
        public double? PlanPostPromoEffectQty { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Status { get; set; }
        public bool? InOut { get; set; }
    }
}
