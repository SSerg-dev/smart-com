using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;
namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(MechanicType))]
    public class HistoricalMechanicType : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }
        public double? Discount { get; set; }
        public string ClientTreeFullPathName { get; set; }
    }
}
