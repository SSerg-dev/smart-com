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
    public class COGS: IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public System.Guid Id {get; set;}
        public bool Disabled {get; set;}
        public DateTimeOffset? DeletedDate {get; set;}

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public float LVSpercent { get; set; }

        public int ClientTreeId { get; set; }
        public System.Guid? BrandTechId { get; set; }

        public virtual BrandTech BrandTech { get; set; }
        public virtual ClientTree ClientTree { get; set; }

    }
}
