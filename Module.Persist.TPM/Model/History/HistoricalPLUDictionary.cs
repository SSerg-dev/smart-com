using Core.History;
using Module.Persist.TPM.Model.TPM;
using System;

namespace Module.Persist.TPM.Model.History
{
	[AssociatedWith(typeof(PLUDictionary))]
	public class HistoricalPLUDictionary : BaseHistoricalEntity<System.Guid>
	{
		public int ClientTreeId { get; set; }

		public string EAN_PC { get; set; }

		public string PluCode { get; set; }

		public string ClientTreeName { get; set; }

		public int ObjectId { get; set; }
	}
}
