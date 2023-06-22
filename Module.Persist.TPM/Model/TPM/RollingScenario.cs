using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Module.Persist.TPM.Enum;

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
        public ScenarioType ScenarioType { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }

        public bool IsMLmodel { get; set; }
        [StringLength(100)]
        public string TaskStatus { get; set; }
        public Guid? FileBufferId { get; set; }
        public Guid? HandlerId { get; set; }

        public bool IsSendForApproval  { get; set; }
        public bool IsCMManagerApproved { get; set; }

        [StringLength(100)]
        public string RSstatus { get; set; }

        [ForeignKey("ClientTree")]
        public int ClientTreeId { get; set; }
        public ClientTree ClientTree { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<Promo> Promoes { get; set; }
    }
}
