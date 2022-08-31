using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class MechanicType : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id {get; set;}

        public bool Disabled {get; set;}

        [Index("Unique_MechanicType", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate {get; set;}

        [Index("Unique_MechanicType", 2, IsUnique = true)]
        [StringLength(255)]
        [Required]
        public string Name {get; set;}

        public double? Discount { get; set; }

        [Index("Unique_MechanicType", 3, IsUnique = true)]
        public int? ClientTreeId { get; set; }
        public ClientTree ClientTree { get; set; }

        public ICollection<NoneNego> NoneNegoes { get; set; }
    }
}
