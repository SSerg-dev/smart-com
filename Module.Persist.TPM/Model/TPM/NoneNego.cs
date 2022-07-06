using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data;

namespace Module.Persist.TPM.Model.TPM
{
    public class NoneNego : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public double? Discount { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }

        public Guid? MechanicId { get; set; }
        public virtual Mechanic Mechanic { get; set; }
        public Guid? MechanicTypeId { get; set; }
        public virtual MechanicType MechanicType { get; set; }
        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
        public int ProductTreeId { get; set; }
        public virtual ProductTree ProductTree { get; set; }
    }
}
