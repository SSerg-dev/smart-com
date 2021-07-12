using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.Import
{
	public class ImportPLUDictionary : BaseImportEntity
	{
		[ImportCSVColumn(ColumnNumber = 0 )]
		[Display(Name = "Client id")]
		public int ClientTreeId { get; set; }

		[ImportCSVColumn(ColumnNumber = 1)]
		[Display(Name = "PLU")]
		public string PLU { get; set; }

		[ImportCSVColumn(ColumnNumber = 2)]
		[Display(Name = "EAN PC")]
		public string EAN_PC { get; set; }
	}
}
