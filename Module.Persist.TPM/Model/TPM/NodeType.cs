using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class NodeType : IEntity<Guid>, IDeactivatable {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [Index]
        public bool Disabled { get; set; }
        [Index("NTX", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        /// <summary>
        /// “ип узла - Product/Client        
        /// </summary>
        [StringLength(50)]
        [Index("NTX", 2, IsUnique = true)]
        [Required]
        public string Type { get; set; }
        [StringLength(255)]
        [Index("NTX", 3, IsUnique = true)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int Priority { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
