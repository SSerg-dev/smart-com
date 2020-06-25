using Core.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.TPM
{
    public class ImportTradeInvestment : BaseImportEntity {

        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "StartDate")]
        public DateTimeOffset? StartDate
        {
            get { return startDate; }
            set { startDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "EndDate")]
        public DateTimeOffset? EndDate
        {
            get { return endDate; }
            set { endDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Client hierarchy")]
        public String ClientTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Client hierarchy code")]
        public int ClientTreeObjectId { get; set; }



        [ImportCSVColumn(ColumnNumber = 4)]
        [NavigationPropertyMap(LookupEntityType = typeof(BrandTech), LookupPropertyName = "BrandsegTechsub")]
        [Display(Name = "BrandTech.BrandsegTechsub")]
        public string BrandsegTechsub { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "TIType")]
        public string TIType { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "TISubType")]
        public string TISubType { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "SizePercent")]
        public float SizePercent { get; set; }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "Marc Calc ROI")]
        public String MarcCalcROI { get; set; }

        [ImportCSVColumn(ColumnNumber = 9)]
        [Display(Name = "Marc Calc Budgets")]
        public String MarcCalcBudgets { get; set; }

        public bool MarcCalcROIBool { get; set; }
        public bool MarcCalcBudgetsBool { get; set; }
        public int ClientTreeId { get; set; }
        public System.Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }

        private DateTimeOffset? startDate;
        private DateTimeOffset? endDate;
    }
}
