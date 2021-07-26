using Core.Data;
using Module.Persist.TPM.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
	/// <summary>
	/// Справочник кодов PLU
	/// </summary>
	public class Plu : IEntity
	{
		/// <summary>
		/// Ссылка на клиента
		/// </summary>
		[Key, Column(Order = 1)] 
		public int ClientTreeId { get; set; }

		/// <summary>
		/// Ссылка на продукт
		/// </summary>
		[Key, Column(Order = 2)]
		[StringLength(255)]
		public string EAN_PC { get; set; }

		/// <summary>
		/// Код PLU
		/// </summary>
		[StringLength(20)]
		public string PluCode { get; set; }
		
		[SpecialNotKeyProperty]
		public virtual ClientTree ClientTree { get; set; }

	}

	public class PromoProduct2Plu : IEntity
	{
		[Key]
		public Guid Id { get; set; }

		public string PluCode { get; set; }

	}

	public class AssortmentMatrix2Plu :IEntity<Guid>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public int ClientTreeId { get; set; }
		public string PluCode { get; set; }
	}

}
