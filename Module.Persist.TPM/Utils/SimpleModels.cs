using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public class SimplePromoPromoProduct
    {
        public Guid Id { get; set; }
        public string PromoStatusName { get; set; }
		public int? ClientTreeId { get; set; }
		public int? ClientTreeKeyId { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
		public DateTimeOffset? DispatchesStart { get; set; }
		public DateTimeOffset? DispatchesEnd { get; set; }
		public bool? InOut { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public string ZREP { get; set; }
        public double? PlanProductIncrementalCaseQty { get; set; }
        public double? PlanProductBaselineCaseQty { get; set; }
        public double? PlanProductPostPromoEffectLSVW1 { get; set; }
        public double? PlanProductPostPromoEffectLSVW2 { get; set; }
        public double? PlanProductBaselineLSV { get; set; }
    }
}
