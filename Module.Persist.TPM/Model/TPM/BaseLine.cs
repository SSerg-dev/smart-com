using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BaseLine : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        [Index("Unique_BaseLine", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_BaseLine", 2, IsUnique = true)]
        public Guid ProductId { get; set; }

        [Index("Unique_BaseLine", 3, IsUnique = true)]
        [Required]
        public DateTimeOffset? StartDate { get; set; }

        [Required]
        [Range(0, 10000000000)]
        public double? QTY { get; set; }

        [Required]
        [Range(0, 10000000000)]
        public double? Price { get; set; }

        [Required]
        [Range(0, 10000000000)]
        public double? BaselineLSV { get; set; }

        [StringLength(255)]
        [Index("Unique_BaseLine", 4, IsUnique = true)]
        public string DemandCode { get; set; }

        [Required]
        public int? Type { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
