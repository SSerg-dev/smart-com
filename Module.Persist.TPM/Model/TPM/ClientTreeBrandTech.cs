using Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Module.Persist.TPM.Model.TPM
{
    public class ClientTreeBrandTech : IEntity<Guid>, IDeactivatable
    {
        public Guid Id { get; set; }
        public int ClientTreeId { get; set; }
        public Guid BrandTechId { get; set; }
        public double Share { get; set; }
        public string ParentClientTreeDemandCode { get; set; }
        public string CurrentBrandTechName { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public virtual ClientTree ClientTree { get; set; }
        public virtual BrandTech BrandTech { get; set; }
    }
}