using Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductCorrectionPriceIncrease : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public double? PlanProductUpliftPercentCorrected { get; set; }

        public string TempId { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }

        public PromoProductPriceIncrease PromoProductPriceIncrease { get; set; }
    }
}
