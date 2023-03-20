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
    public class PromoDemand : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid BrandTechId { get; set; }
        public Guid MechanicId { get; set; }
        public Guid? MechanicTypeId { get; set; }
        public int BaseClientObjectId { get; set; }

        public int Discount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Account { get; set; }
        public string Week { get; set; }
        public DateTimeOffset MarsStartDate { get; set; }
        public DateTimeOffset MarsEndDate { get; set; }
        public double Baseline { get; set; }
        public double Uplift { get; set; }
        public double Incremental { get; set; }
        public double Activity { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public virtual BrandTech BrandTech { get; set; }
        public virtual Mechanic Mechanic { get; set; }
        public virtual MechanicType MechanicType { get; set; }
    }
}
