using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class ClientTree : ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index("CX_ObjDate", 1, IsUnique = true)]
        public int ObjectId { get; set; }
        public int parentId { get; set; }
        public int depth { get; set; }
        public string Type { get; set; }
        public string RetailTypeName { get; set; }
        public string Name { get; set; }
        public string FullPathName { get; set; }
        public DateTime StartDate { get; set; }
        [Index("CX_ObjDate", 2, IsUnique = true)]
        public DateTime? EndDate { get; set; }
        public short Share { get; set; }
        [StringLength(255)]
        public string ExecutionCode { get; set; }
        [StringLength(255)]
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
