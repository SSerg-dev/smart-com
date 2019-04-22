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
    public class MechanicType : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id {get; set;}

        public bool Disabled {get; set;}

        public DateTimeOffset? DeletedDate {get; set;}
        [StringLength(255)]
        [Required]
        public string Name {get; set;}
        public int? Discount { get; set; }
    }
}
