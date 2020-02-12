using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class BaseTradeInvestment : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; } = Guid.NewGuid();
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public int ClientTreeId { get; set; }
        public System.Guid? BrandTechId { get; set; }

        [StringLength(255)]
        public string TIType { get; set; }
        [StringLength(255)]
        public string TISubType { get; set; }
        public float SizePercent { get; set; }
        public bool MarcCalcROI { get; set; }
        public bool MarcCalcBudgets { get; set; }
        public int Year { get; set; }

        public virtual BrandTech BrandTech { get; set; }
        public virtual ClientTree ClientTree { get; set; }
    }

    public class TradeInvestment : BaseTradeInvestment { }

    public class ActualTradeInvestment : BaseTradeInvestment
    {
        public bool IsTIIncidentCreated { get; set; }
    }
}