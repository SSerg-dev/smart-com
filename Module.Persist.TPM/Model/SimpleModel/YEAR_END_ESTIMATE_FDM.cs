using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class YEAR_END_ESTIMATE_FDM
    {
        [StringLength(50)]
        public string YEAR { get; set; }

        [StringLength(100)]
        public string BRAND_SEG_TECH_CODE { get; set; }

        [StringLength(100)]
        public string G_HIERARCHY_ID { get; set; }

        public double DMR_PLAN_LSV { get; set; }

        public double YTD_LSV { get; set; }

        public double YEE_LSV { get; set; }
    }
}
