using Core.Data;
using Core.Export;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProductDifference: IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        [ExportCSVColumn(ColumnNumber = 0)]
        public string DemandUnit { get; set; }
        [ExportCSVColumn(ColumnNumber = 1)]
        public string DMDGROUP { get; set; }
        [ExportCSVColumn(ColumnNumber = 2)]
        public string LOC { get; set; }
        [ExportCSVColumn(ColumnNumber = 3)]
        public string StartDate { get; set; }
        [ExportCSVColumn(ColumnNumber = 4)]
        public int? DURInMinutes{ get; set; }
        [ExportCSVColumn(ColumnNumber = 5)]
        public string Type { get; set; }
        [ExportCSVColumn(ColumnNumber = 6)]
        public string ForecastID{ get; set; }
        [ExportCSVColumn(ColumnNumber = 7)]
        public decimal? QTY { get; set; }
        [ExportCSVColumn(ColumnNumber = 8)]
        public string MOE { get; set; }
        [ExportCSVColumn(ColumnNumber = 9)]
        public string Source { get; set; }
        [ExportCSVColumn(ColumnNumber = 10)]
        public string SALES_ORG { get; set; }
        [ExportCSVColumn(ColumnNumber = 11)]
        public string SALES_DIST_CHANNEL { get; set; }
        [ExportCSVColumn(ColumnNumber = 12)]
        public string SALES_DIVISON { get; set; }
        [ExportCSVColumn(ColumnNumber = 13)]
        public string BUS_SEG { get; set; }
        [ExportCSVColumn(ColumnNumber = 14)]
        public string MKT_SEG { get; set; }
        [ExportCSVColumn(ColumnNumber = 15)]
        public string DELETION_FLAG { get; set; }

        [ExportCSVColumn(ColumnNumber = 16)]
        public string DELETION_DATE { get; set; }

        [ExportCSVColumn(ColumnNumber = 17)]
        public string INTEGRATION_STAMP { get; set; }
        [ExportCSVColumn(ColumnNumber = 18)]
        public string Roll_FC_Flag { get; set; }

        [ExportCSVColumn(ColumnNumber = 19)]
        public string Promotion_Start_Date { get; set; }
        [ExportCSVColumn(ColumnNumber = 20)]
        public int? Promotion_Duration { get; set; }
        [ExportCSVColumn(ColumnNumber = 21)]
        public string Promotion_Status { get; set; }
        [ExportCSVColumn(ColumnNumber = 22)]
        public string Promotion_Campaign { get; set; }
    }
}
