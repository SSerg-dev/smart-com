using Core.Data;
using Core.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
	public class PLUDictionary : IEntity<Guid>
	{
		//[Key]
		public Guid Id { get; set; }

		[Key, Column(Order = 0)]
		public int ClientTreeId { get; set; }
		[Key, Column(Order = 1)]
		public string EAN_PC { get; set; }

		public int ObjectId { get; set; }

		public string PluCode { get; set; }
		public string ClientTreeName { get; set; }

	}

	[AssociatedWith(typeof(PLUDictionary))]
	public class HistoryPLUDictionary : BaseHistoricalEntity<System.Guid>
	{
		public int ClientTreeId { get; set; }

		//[Key, Column(Order = 1)]
		public Guid ProductId { get; set; }
		public int ObjectId { get; set; }

		public string PluCode { get; set; }
		public string EAN_PC { get; set; }
		public string ClientTreeName { get; set; }
		public string ProductEN { get; set; }
	}
}
