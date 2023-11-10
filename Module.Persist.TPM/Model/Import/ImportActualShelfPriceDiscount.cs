using Core.Import;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportActualShelfPriceDiscount : BaseImportEntity
    {      

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo ID")]
        public int? PromoId { get; set; }       

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Mechanic")]
        public string Mechanic { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Mechanic Type")]
        public string MechanicType { get; set; }       

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Discount %")]
        public double? Discount { get; set; }  
        
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Shelf Price")]
        public double? ShelfPrice { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Invoice number")]
        public string InvoiceNumber { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Document number")]
        public string DocumentNumber{ get; set; }
    }
}
