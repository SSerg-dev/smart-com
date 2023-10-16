using Core.Import;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportRpaTLCdraft : BaseImportEntity
    {
        private DateTimeOffset startDate;
        private DateTimeOffset endDate;
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Promo Type")]
        public string PromoType { get; set; }
        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client")]
        public string Client { get; set; }
        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy code")]
        public int ClientHierarchyCode { get; set; }
        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "BrandTech")]
        public string BrandTech { get; set; }
        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Subrange")]
        public string Subrange { get; set; }
        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Mechanic")]
        public string Mechanic { get; set; }
        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Mechanic Type")]
        public string MechanicType { get; set; }
        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "Mechanic comment")]
        public string MechanicComment { get; set; }
        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Discount %")]
        public double Discount { get; set; }
        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Promo Start Date")]
        public DateTimeOffset PromoStartDate
        {
            get { return startDate; }
            set { startDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }
        [ImportCSVColumn(ColumnNumber = 10)]
        [Display(Name = "Promo End Date")]
        public DateTimeOffset PromoEndDate
        {
            get { return endDate; }
            set { endDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }
        [ImportCSVColumn(ColumnNumber = 11)]
        [Display(Name = "Budget Year")]
        public int BudgetYear { get; set; }

        [ImportCSVColumn(ColumnNumber = 12)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [ImportCSVColumn(ColumnNumber = 13)]
        [Display(Name = "Promo Start Date")]
        public DateTimeOffset DispatchStartDate
        {
            get { return startDate; }
            set { startDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        [ImportCSVColumn(ColumnNumber = 14)]
        [Display(Name = "Promo End Date")]
        public DateTimeOffset DispatchEndDate
        {
            get { return endDate; }
            set { endDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }
    }
}
