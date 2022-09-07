using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class RATIShopper : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public System.Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public int Year { get; set; }
        public float RATIShopperPercent { get; set; }

        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
    }
}
