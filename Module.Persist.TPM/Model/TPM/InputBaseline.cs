using Core.Data;
using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class InputBaseline : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ImportId { get; set; }

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "REP")]
        public string REP { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "DMD Group")]
        public string DMDGroup { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "LOC")]
        public string LOC { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Start date")]
        public DateTimeOffset STARTDATE { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Dur in Minutes")]
        public int DurInMinutes { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "QTY")]
        public double QTY { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "MOE")]
        public int MOE { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "SALES_ORG")]
        public int SALES_ORG { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "SALES DIST.CHANNEL")]
        public int SALES_DIST_CHANNEL { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "SALES_DIVISON")]
        public int SALES_DIVISON { get; set; }

        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "BUS_SEG")]
        public int BUS_SEG { get; set; }

        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "MKT_SEG")]
        public int MKT_SEG { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "DELETION FLAG")]
        public string DELETION_FLAG { get; set; }

        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "DELETION DATE")]
        public DateTimeOffset DELETION_DATE { get; set; }

        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "INTEGRATION_STAMP")]
        public DateTimeOffset INTEGRATION_STAMP { get; set; }
    }
}