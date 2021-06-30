﻿using Core.Data;
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
		public Guid ProductId { get; set; }

		/// <summary>
		/// Код PLU
		/// </summary>
		public string PluCode { get; set; }
	}
}
