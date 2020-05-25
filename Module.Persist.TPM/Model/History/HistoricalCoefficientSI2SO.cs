using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(CoefficientSI2SO))]
    public class HistoricalCoefficientSI2SO : BaseHistoricalEntity<System.Guid>
    {
        public string DemandCode { get; set; }
        public double CoefficientValue { get; set; }
        public bool? Lock { get; set; }
        public string BrandTechName { get; set; }
        public string BrandTechBrandTech_code { get; set; }
    }
}
