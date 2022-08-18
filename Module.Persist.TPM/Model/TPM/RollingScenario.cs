using Core.Data;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class RollingScenario : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Index(IsUnique = true)]
        public int? RSId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid CreatorId { get; set; } // в Core нельзя обратиться к User на прямую
        public string CreatorLogin { get; set; }

        [ForeignKey("PromoStatus")]
        public Guid PromoStatusId { get; set; }
        public PromoStatus PromoStatus { get; set; }

        [ForeignKey("ClientTree")]
        public int ClientTreeId { get; set; }
        public ClientTree ClientTree { get; set; }

        public ICollection<Promo> Promoes { get; set; }
    }
}
