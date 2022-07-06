using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Mechanic : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("Unique_Mechanic", 1, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_Mechanic", 2, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Required]
        [Index("Unique_Mechanic", 3, IsUnique = true)]
        public string Name { get; set; }
        [StringLength(255)]
        public string SystemName { get; set; }

        [Index("Unique_Mechanic", 4, IsUnique = true)]
        public Guid? PromoTypesId { get; set; }
        public virtual PromoTypes PromoTypes { get; set; }

        public virtual ICollection<NoneNego> NoneNegoes { get; set; }
    }
}
