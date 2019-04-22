using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductTree : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Index("Unique_PromoProductTree", 3, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_PromoProductTree", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_PromoProductTree", 1, IsUnique = true)]
        public Guid PromoId { get; set; }
        [Index("Unique_PromoProductTree", 2, IsUnique = true)]
        public int ProductTreeObjectId { get; set; }

        public virtual Promo Promo { get; set; }
    }
}
