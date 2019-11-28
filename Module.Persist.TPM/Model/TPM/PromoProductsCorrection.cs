using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductsCorrection : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid PromoProductId { get; set; }
        public double? PlanProductUpliftPercentCorrected { get; set; }

        public string TempId { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }


        public virtual PromoProduct PromoProduct { get; set; }

    }
}
