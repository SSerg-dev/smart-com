using Core.Data;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class SavedScenario : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public DateTimeOffset CreateDate { get; set; } = TimeHelper.Now();
        [StringLength(512)]
        public string ScenarioName { get; set; }

        [ForeignKey("RollingScenario")]
        public Guid RollingScenarioId { get; set; }
        public virtual RollingScenario RollingScenario { get; set; }
        public ICollection<Promo> Promoes { get; set; }
    }
}
