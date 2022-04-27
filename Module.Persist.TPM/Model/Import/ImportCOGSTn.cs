using Core.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportCOGSTn : BaseImportEntity
    {
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
        [Display(Name = "BrandTech")]
        public String BrandsegTechsub { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Volume")]
        public double Volume { get; set; }

        public int ClientTreeId { get; set; }
        public Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }

        private DateTimeOffset? startDate;
        private DateTimeOffset? endDate;
    }
}
