using Core.Data;
using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportClientsShare : BaseImportEntity, IEntity<Guid>
    {

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Demand code")]
        public string DemandCode { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Base client")]
        public string Client { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Brand tech")]
        public string BrandTech { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Share")]
        public double LeafShare { get; set; }
    }
}
