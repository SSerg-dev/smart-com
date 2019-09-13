using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.DTO
{
    public class PlanPostPromoEffectReportWeekView :IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ZREP { get; set; }
        public string DemandCode { get; set; }
        public string PromoNameId { get; set; }
        public string LocApollo { get; set; }
        public string TypeApollo { get; set; }
        public string ModelApollo { get; set; }
        public DateTimeOffset? WeekStartDate { get; set; }
        public double? PlanUplift { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public string Week { get; set; }
        public string Status { get; set; }
        public double? PlanPostPromoEffectQtyW1 { get; set; }
        public double? PlanProductBaselineCaseQtyW1 { get; set; }
        public double? PlanProductPostPromoEffectLSVW1 { get; set; }
        public double? PlanProductBaselineLSVW1 { get; set; }
        public double? PlanPostPromoEffectQtyW2 { get; set; }
        public double? PlanProductBaselineCaseQtyW2 { get; set; }
        public double? PlanProductPostPromoEffectLSVW2 { get; set; }
        public double? PlanProductBaselineLSVW2 { get; set; }
        public bool? InOut { get; set; }
    }
}
