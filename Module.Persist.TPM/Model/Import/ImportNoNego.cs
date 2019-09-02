using Core.Import;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import {
    public class ImportNoNego : BaseImportEntity {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "ClientHierarchy")]
        public String ClientTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Client hierarchy code")]
        public int ClientObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Product hierarchy")]
        public String ProductTreeFullPath { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Product hierarchy code")]
        public int ProductObjectId { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [NavigationPropertyMap(LookupEntityType = typeof(Mechanic), LookupPropertyName = "Name")]
        [Display(Name = "Mechanic")]
        public String MechanicName { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Mechanic Type")]
        [NavigationPropertyMap(LookupEntityType = typeof(MechanicType), LookupPropertyName = "Name")]
        public String MechanicTypeName { get; set; }

        [ImportCSVColumn(ColumnNumber = 6)]
        [Display(Name = "Mechanic Discount")]
        public int? MechanicDiscount { get; set; }

        [ImportCSVColumn(ColumnNumber = 7)]
        [Display(Name = "From Date")]
        public DateTimeOffset? FromDate
        {
            get { return fromDate; }
            set { fromDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        [ImportCSVColumn(ColumnNumber = 8)]
        [Display(Name = "To Date")]
        public DateTimeOffset? ToDate
        {
            get { return toDate; }
            set { toDate = ChangeTimeZoneUtil.ResetTimeZone(value); }
        }

        public int ClientTreeId { get; set; }
        public int ProductTreeId { get; set; }
        public System.Guid? MechanicId { get; set; }
        public System.Guid? MechanicTypeId { get; set; }
        public virtual Mechanic Mechanic { get; set; }
        public virtual MechanicType MechanicType { get; set; }


        private DateTimeOffset? fromDate;
        private DateTimeOffset? toDate;
    }
}
