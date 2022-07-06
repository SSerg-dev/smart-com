using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class Color : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public System.Guid Id { get; set; }
        [Index("Unique_Color", 2, IsUnique = true)]
        public bool Disabled { get; set; }
        [Index("Unique_Color", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [StringLength(10)]
        public string SystemName { get; set; }
        [Index]
        [StringLength(255)]
        public string DisplayName { get; set; }

        [Index("Unique_Color", 1, IsUnique = true)]
        public Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}
