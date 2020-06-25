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

        private string brandTechName;
        public string BrandTechName
        {
            get
            {
                return string.IsNullOrEmpty(BrandTechBrandsegTechsub)
               ? brandTechName
               : BrandTechBrandsegTechsub;
            }
            set
            {
                brandTechName = value;
            }
        }
        public string BrandTechBrandsegTechsub { get; set; }

        private string brandTechBrandTech_code;
        public string BrandTechBrandTech_code
        {
            get
            {
                return string.IsNullOrEmpty(BrandTechBrandsegTechsub_code)
                ? brandTechBrandTech_code
                : BrandTechBrandsegTechsub_code;
            }
            set
            {
                brandTechBrandTech_code = value;
            }
        }
        public string BrandTechBrandsegTechsub_code { get; set; }
    }
}
