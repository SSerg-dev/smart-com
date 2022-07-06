using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoTypes : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index("Unique_Name", 2, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_Name", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(255)]
        [Index("Unique_Name", 1, IsUnique = true)]
        [Required]
        public string Name { get; set; }
        public string Glyph { get; set; }
        [StringLength(255)]
        public string SystemName { get; set; }

        public virtual ICollection<Mechanic> Mechanics { get; set; }
    }
}
