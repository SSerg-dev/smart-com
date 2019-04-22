using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(Event))]
    public class HistoricalEvent : BaseHistoricalEntity<System.Guid>
    {
        public string Name { get; set; }

        public int Year { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
    }
}
