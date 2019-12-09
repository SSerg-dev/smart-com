using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;

namespace Module.Persist.TPM.Model.TPM
{
    public class ClientTreeNeedUpdateIncident : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
