using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class IncreaseBaseLine: IEntity<Guid>, IDeactivatable, ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool Disabled { get; set; }

        [Index("Unique_BaseLine", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_BaseLine", 2, IsUnique = true)]
        public Guid ProductId { get; set; }

        [Index("Unique_BaseLine", 3, IsUnique = true)]
        [Required]
        public DateTimeOffset? StartDate { get; set; }

        [StringLength(255)]
        [Index("Unique_BaseLine", 4, IsUnique = true)]
        public string DemandCode { get; set; }

        [Range(0, 10000000000)]
        public double? InputBaselineQTY { get; set; }

        [Range(0, 10000000000)]
        public double? SellInBaselineQTY { get; set; }

        [Range(0, 10000000000)]
        public double? SellOutBaselineQTY { get; set; }

        [Required]
        public int? Type { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public virtual Product Product { get; set; }

        public object Clone()
        {
            return new IncreaseBaseLine()
            {
                Id = this.Id,
                DeletedDate = this.DeletedDate,
                ProductId = this.ProductId,
                Product = this.Product,
                DemandCode = this.DemandCode,
                InputBaselineQTY = this.InputBaselineQTY,
                SellInBaselineQTY = this.SellInBaselineQTY,
                SellOutBaselineQTY = this.SellOutBaselineQTY,
                Type = this.Type,
                LastModifiedDate = this.LastModifiedDate,
                Disabled = this.Disabled
            };
        }
    }
}
