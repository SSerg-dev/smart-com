using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(ClientTreeBrandTech))]
    public class HistoricalClientTreeBrandTech : BaseHistoricalEntity<System.Guid>
    {
        public double Share { get; set; }
        public string ParentClientTreeDemandCode { get; set; }
        public string CurrentBrandTechName { get; set; }
        public string ClientTreeName { get; set; }
        public int ClientTreeObjectId { get; set; }
    }
}
