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
        public string ClientTreeName { get; set; }
        public string EAN_PC { get; set; }
    }
}
