using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CoefficientSI2SO : IEntity<Guid>, IDeactivatable, ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        [Index("Unique_Coef", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Index("Unique_Coef", 2, IsUnique = true)]
        public string DemandCode { get; set; }
        public bool? Lock { get; set; }
        public double? CoefficientValue { get; set; }
        public bool NeedProcessing { get; set; }

        [Index("Unique_Coef", 3, IsUnique = true)]
        public Guid BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public object Clone()
        {
            return new CoefficientSI2SO()
            {
                Id = this.Id,
                Disabled = this.Disabled,
                DeletedDate = this.DeletedDate,
                DemandCode = this.DemandCode,
                Lock = this.Lock,
                CoefficientValue = this.CoefficientValue,
                BrandTechId = this.BrandTechId,
                BrandTech = this.BrandTech,
                NeedProcessing = this.NeedProcessing
            };
        }
    }
}