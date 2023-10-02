using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ClientTreeBrandTech : IEntity<Guid>, IDeactivatable
    {
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        [Index("ClientTreeBrandTech__IDX", 4, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        public double Share { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        [Index("ClientTreeBrandTech__IDX", 3, IsUnique = true)]
        public string ParentClientTreeDemandCode { get; set; }
        public string CurrentBrandTechName { get; set; }

        [Index("ClientTreeBrandTech__IDX", 1, IsUnique = true)]
        public int ClientTreeId { get; set; }
        public virtual ClientTree ClientTree { get; set; }
        [Index("ClientTreeBrandTech__IDX",2 , IsUnique = true)]
        public Guid BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}