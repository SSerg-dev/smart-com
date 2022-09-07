using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductsCorrection : IEntity<Guid>, IDeactivatable, IMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        [Index("Unique_PromoProductsCorrection", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [Index("Unique_PromoProductsCorrection", 2, IsUnique = true)]
        public TPMmode TPMmode { get; set; }

        public double? PlanProductUpliftPercentCorrected { get; set; }

        public string TempId { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }
        [Index("Unique_PromoProductsCorrection", 3, IsUnique = true)]
        public Guid PromoProductId { get; set; }
        public virtual PromoProduct PromoProduct { get; set; }

    }
}
