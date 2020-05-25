using System;

namespace Module.Persist.TPM.Model.TPM {
    public class SchedulerClientTreeDTO : ICloneable
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int parentId { get; set; }
        public int depth { get; set; }
        public string Type { get; set; }
        public string RetailTypeName { get; set; }
        public string Name { get; set; }
        public string FullPathName { get; set; } 
        public bool? IsOnInvoice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short Share { get; set; }
        public string GHierarchyCode { get; set; }
        public string DemandCode { get; set; }
        public bool IsBaseClient { get; set; }

        //Dispatch start
        public bool? IsBeforeStart { get; set; }
        public int? DaysStart { get; set; }
        public bool? IsDaysStart { get; set; }

        //Dispatch end
        public bool? IsBeforeEnd { get; set; }
        public int? DaysEnd { get; set; }
        public bool? IsDaysEnd { get; set; }

        public double? PostPromoEffectW1 { get; set; }
        public double? PostPromoEffectW2 { get; set; }

        public string LogoFileName { get; set; }

        public string TypeName { get; set; }
        public string InOutId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
