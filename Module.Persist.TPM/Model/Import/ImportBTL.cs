using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportBTL : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Number")]
        public int Number { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "PlanBTLTotal")]
        public double? PlanBTLTotal { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "ActualBTLTotal")]
        public double? ActualBTLTotal { get; set; }
        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "StartDate")]
        public DateTimeOffset? StartDate { get; set; }
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "EndDate")]
        public DateTimeOffset? EndDate { get; set; }
    }
}
