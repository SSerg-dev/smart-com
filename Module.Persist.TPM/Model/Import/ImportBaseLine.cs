using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportBaseLine : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "ZREP")]
        public string ProductZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client Demand Code")]
        public string ClientTreeDemandCode { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client Demand Code")]
        public string ModelNoWhere { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Start date")]
        public DateTimeOffset? StartDate { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "QTY")]
        [Range(0, 10000000000)]
        public double? QTY { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Type")]
        public int? Type { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Price")]
        [Range(0, 10000000000)]
        public double? Price { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Baseline, LSV")]
        [Range(0, 10000000000)]
        public double? BaselineLSV { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "QTY")]
        public string QTYNoWhere { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Type")]
        public string TypeNoWhere { get; set; }

        public int ClientTreeId { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
