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
    public class CurrentDayIncremental : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public DateTimeOffset LastChangeDate { get; set; }
        [MaxLength(12)]
        public string WEEK { get; set; }
        public float IncrementalQty { get; set; }
        [MaxLength(255)]
        public string DemandCode { get; set; }
        public string DMDGroup { get; set; }
        [Index("Unique_PromoProduct", 0, IsUnique = true)]
        public Guid PromoId { get; set; }
        public Promo Promo { get; set; }
        [Index("Unique_PromoProduct", 1, IsUnique = true)]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
