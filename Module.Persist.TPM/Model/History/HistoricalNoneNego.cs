using System;
using Module.Persist.TPM.Model.TPM;
using Core.History;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(NoneNego))]
    public class HistoricalNoneNego : BaseHistoricalEntity<Guid>
    {
        public double? Discount { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public string MechanicName { get; set; }
        public string MechanicTypeName { get; set; }
        public string ClientTreeFullPathName { get; set; }
        public string ProductTreeFullPathName { get; set; }
        public int ClientTreeObjectId { get; set; }
        public int ProductTreeObjectId { get; set; }
    }
}