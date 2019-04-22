using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class Brand : IEntity<Guid>, IDeactivatable
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
    }
}
