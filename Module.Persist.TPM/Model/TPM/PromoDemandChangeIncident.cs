using System;
using Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class PromoDemandChangeIncident : IEntity<Guid> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int? PromoIntId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        public string ClientHierarchy { get; set; }
        public string BrandTech { get; set; }
        public string PromoStatus { get; set; }

        [StringLength(255)]
        public string OldMarsMechanic { get; set; }
        [StringLength(255)]
        public string NewMarsMechanic { get; set; }

        public double? OldMarsMechanicDiscount { get; set; }
        public double? NewMarsMechanicDiscount { get; set; }

        public DateTimeOffset? OldDispatchesStart { get; set; }
        public DateTimeOffset? NewDispatchesStart { get; set; }
     
        public double? OldPlanPromoUpliftPercent { get; set; }
        public double? NewPlanPromoUpliftPercent { get; set; }

        public double? OldPlanPromoIncrementalLSV { get; set; }
        public double? NewPlanPromoIncrementalLSV { get; set; }
		
		// Post poromo effect
        public int? OldPlanSteel { get; set; }
        public int? NewPlanSteel { get; set; }

		// Флаг, указывающий на то, что это новое промо
		public bool IsCreate { get; set; }
		// Флаг, указывающий на то, что это удалённое промо
		public bool IsDelete { get; set; }
		// Флаг, указывающий на то, что изменился список продуктов
		public bool IsProductListChange { get; set; }
		// Дата обработки записи (отправки нотификации)
		public DateTimeOffset? ProcessDate { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public PromoDemandChangeIncident() { }

		/// <summary>
		/// Конструктор для создания записи об изменении промо
		/// </summary>
		/// <param name="oldPromo"></param>
		/// <param name="newPromo"></param>
		public PromoDemandChangeIncident(Promo oldPromo, Promo newPromo) {
            PromoIntId = newPromo.Number;
            Name = newPromo.Name;
            ClientHierarchy = newPromo.ClientHierarchy;
            BrandTech = newPromo.BrandTech != null ? newPromo.BrandTech.BrandsegTechsub : null;
			PromoStatus = newPromo.PromoStatus.SystemName != null ? newPromo.PromoStatus.SystemName : null;
			OldMarsMechanic = oldPromo.MarsMechanic != null ? oldPromo.MarsMechanic.Name : null;
            NewMarsMechanic = newPromo.MarsMechanic != null ? newPromo.MarsMechanic.Name : null;
            OldMarsMechanicDiscount = oldPromo.MarsMechanicDiscount;
            NewMarsMechanicDiscount = newPromo.MarsMechanicDiscount;
            OldDispatchesStart = oldPromo.DispatchesStart;
            NewDispatchesStart = newPromo.DispatchesStart;
            OldPlanPromoUpliftPercent = oldPromo.PlanPromoUpliftPercent;
            NewPlanPromoUpliftPercent = newPromo.PlanPromoUpliftPercent;
            OldPlanPromoIncrementalLSV = oldPromo.PlanPromoIncrementalLSV;
            NewPlanPromoIncrementalLSV = newPromo.PlanPromoIncrementalLSV;
			IsCreate = false;
            IsDelete = false;
        }

		/// <summary>
		/// Конструктор для создания записи об изменении промо
		/// </summary>
		/// <param name="promo"></param>
		/// <param name="isDelete"></param>
		public PromoDemandChangeIncident(Promo promo, bool isDelete = false) {
            PromoIntId = promo.Number;
            Name = promo.Name;
            ClientHierarchy = promo.ClientHierarchy;
            BrandTech = promo.BrandTech != null ? promo.BrandTech.BrandsegTechsub : null;
			PromoStatus = promo.PromoStatus.SystemName != null ? promo.PromoStatus.SystemName : null;
			OldMarsMechanic = isDelete ? (promo.MarsMechanic != null ? promo.MarsMechanic.Name : null) : null;
            NewMarsMechanic = promo.MarsMechanic != null ? promo.MarsMechanic.Name : null;
            OldMarsMechanicDiscount = isDelete ? promo.MarsMechanicDiscount : null;
            NewMarsMechanicDiscount = promo.MarsMechanicDiscount;
            OldDispatchesStart = isDelete ? promo.DispatchesStart : null;
            NewDispatchesStart = promo.DispatchesStart;
            OldPlanPromoUpliftPercent = isDelete ? promo.PlanPromoUpliftPercent : null;
            NewPlanPromoUpliftPercent = promo.PlanPromoUpliftPercent;
            OldPlanPromoIncrementalLSV = isDelete ? promo.PlanPromoIncrementalLSV : null;
            NewPlanPromoIncrementalLSV = promo.PlanPromoIncrementalLSV;
            IsCreate = !isDelete;
            IsDelete = isDelete;
        }

        /// <summary>
		/// Конструктор для создания записи об изменении промо
		/// </summary>
		/// <param name="promo"></param>
		/// <param name="isDelete"></param>
		public PromoDemandChangeIncident (Promo promo, string oldMarsMechanic, double? oldMarsMechanicDiscount, DateTimeOffset? oldDispatchesStart, double? oldPlanPromoUpliftPercent, double? oldPlanPromoIncrementalLSV)
        {
            PromoIntId = promo.Number;
            Name = promo.Name;
            ClientHierarchy = promo.ClientHierarchy;
            BrandTech = promo.BrandTech != null ? promo.BrandTech.BrandsegTechsub : null;
            PromoStatus = promo.PromoStatus.SystemName != null ? promo.PromoStatus.SystemName : null;
            OldMarsMechanic = oldMarsMechanic;
            NewMarsMechanic = promo.MarsMechanic != null ? promo.MarsMechanic.Name : null;
            OldMarsMechanicDiscount = oldMarsMechanicDiscount;
            NewMarsMechanicDiscount = promo.MarsMechanicDiscount;
            OldDispatchesStart = oldDispatchesStart;
            NewDispatchesStart = promo.DispatchesStart;
            OldPlanPromoUpliftPercent = oldPlanPromoUpliftPercent;
            NewPlanPromoUpliftPercent = promo.PlanPromoUpliftPercent;
            OldPlanPromoIncrementalLSV = oldPlanPromoIncrementalLSV;
            NewPlanPromoIncrementalLSV = promo.PlanPromoIncrementalLSV;
            IsCreate = false;
            IsDelete = false;
        }
    }
}
