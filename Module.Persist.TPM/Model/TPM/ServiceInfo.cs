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
    public class ServiceInfo : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [StringLength(255)]
        [Index(IsUnique = true)]
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
