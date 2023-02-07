using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class RollingVolume : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string DemandGroup { get; set; }
        public string DMDGroup { get; set; }
        public string Week { get; set; }
        public double? PlanProductIncrementalQTY { get; set; }
        public double? Actuals { get; set; }
        public double? OpenOrders { get; set; }
        public double? ActualOO { get; set; }
        public double? Baseline { get; set; }
        public double? ActualIncremental { get; set; }
        public double? PreliminaryRollingVolumes { get; set; }
        public double? PreviousRollingVolumes { get; set; }
        public double? PromoDifference { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double? RollingVolumesCorrection { get; set; }
        public double? RollingVolumesTotal { get; set; }
        public double? ManualRollingTotalVolumes { get; set; } 
        public bool Lock { get; set; }
        public double? FullWeekDiff { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
