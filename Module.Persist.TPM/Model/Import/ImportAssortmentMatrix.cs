using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportAssortmentMatrix : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Client hierarchy")]
        public string ClientTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client hierarchy code")]
        public int ClientObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [NavigationPropertyMap(LookupEntityType = typeof(Product), LookupPropertyName = "EAN_PC")]
        [Display(Name = "EAN PC")]
        public string EAN_PC { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Start date")]
        public DateTimeOffset? StartDate
        {
            get { return startDate; }
            set { startDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "End Date")]
        public DateTimeOffset? EndDate
        {
            get { return endDate; }
            set { endDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        public int ClientTreeId { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        private DateTimeOffset? startDate;
        private DateTimeOffset? endDate;
    }
}
