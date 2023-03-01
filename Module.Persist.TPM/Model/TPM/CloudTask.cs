using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class CloudTask : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        [StringLength(255)]
        public string PipeLine { get; set; }
        [StringLength(255)]
        public string Status { get; set; }
        [StringLength(255)]
        public string Model { get; set; } // название класса Json модели
        public string ModelJson { get; set; }
    }
}
