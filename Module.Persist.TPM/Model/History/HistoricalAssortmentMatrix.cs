using Core.Data;
using Core.History;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.History
{
    [AssociatedWith(typeof(AssortmentMatrix))]
    public class HistoricalAssortmentMatrix : BaseHistoricalEntity<System.Guid>
    {
        public int Number { get; set; }
        public string ClientTreeName { get; set; }
        public string ProductEANPC { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
