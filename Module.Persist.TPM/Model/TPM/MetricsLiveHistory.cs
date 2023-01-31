using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class MetricsLiveHistory : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public TypeMetrics Type { get; set; }
        public DateTimeOffset Date { get; set; }
        public int ClientTreeId { get; set; }
        public double Value { get; set; }
        public double ValueLSV { get; set; }
    }
    public enum TypeMetrics
    {
        PPA,
        PCT
    }
}
