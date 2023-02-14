using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductTree : IEntity<Guid>, IDeactivatable, IMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_PromoProductTree", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_PromoProductTree", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        public TPMmode TPMmode { get; set; }

        [Index("Unique_PromoProductTree", 2, IsUnique = true)]
        public int ProductTreeObjectId { get; set; }

        [Index("Unique_PromoProductTree", 1, IsUnique = true)]
        public Guid PromoId { get; set; }
        public virtual Promo Promo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
