using System;
using Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM {
    public class PromoDemandChangeIncident : IEntity<Guid> {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Int32? PromoIntId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        public string ClientHierarchy { get; set; }
        public string BrandTech { get; set; }
        public string OldPromoStatus { get; set; }
        public string NewPromoStatus { get; set; }

        [StringLength(20)]
        public string OldMarsMechanic { get; set; }
        [StringLength(20)]
        public string NewMarsMechanic { get; set; }
        [StringLength(20)]
        public string OldPlanInstoreMechanic { get; set; }
        [StringLength(20)]
        public string NewPlanInstoreMechanic { get; set; }

        public int? OldOutletCount { get; set; }
        public int? NewOutletCount { get; set; }

        public double? OldMarsMechanicDiscount { get; set; }
        public double? NewMarsMechanicDiscount { get; set; }
        public double? OldPlanInstoreMechanicDiscount { get; set; }
        public double? NewPlanInstoreMechanicDiscount { get; set; }


        public DateTimeOffset? OldStartDate { get; set; }
        public DateTimeOffset? NewStartDate { get; set; }
        public DateTimeOffset? OldEndDate { get; set; }
        public DateTimeOffset? NewEndDate { get; set; }

        public DateTimeOffset? OldDispatchesStart { get; set; }
        public DateTimeOffset? NewDispatchesStart { get; set; }
        public DateTimeOffset? OldDispatchesEnd { get; set; }
        public DateTimeOffset? NewDispatchesEnd { get; set; }

        public double? OldPlanPromoUpliftPercent { get; set; }
        public double? NewPlanPromoUpliftPercent { get; set; }

        public double? OldPlanPromoIncrementalLSV { get; set; }
        public double? NewPlanPromoIncrementalLSV { get; set; }

        public int? OldPlanSteel { get; set; }
        public int? NewPlanSteel { get; set; }

        public string OldXSite { get; set; }
        public string NEWXSite { get; set; }

        public string OldCatalogue { get; set; }
        public string NEWCatalogue { get; set; }
        // Флаг; указывающий на то, что это новое промо
        public bool IsCreate { get; set; }
        // Флаг; указывающий на то, что это новое промо
        public bool IsDelete { get; set; }
        // Флаг; указывающий на то, что изменился список продуктов
        public bool IsProductListChange { get; set; }
        // Дата обработки записи (отправки нотификации)
        public DateTimeOffset? ProcessDate { get; set; }

        public PromoDemandChangeIncident() { }

        /// <summary>
        /// Конструктор для создание записи об изменении промо
        /// </summary>
        /// <param name="oldPromo"></param>
        /// <param name="newPromo"></param>
        public PromoDemandChangeIncident(Promo oldPromo, Promo newPromo) {
            PromoIntId = newPromo.Number;
            Name = newPromo.Name;
            ClientHierarchy = newPromo.ClientHierarchy;
            BrandTech = newPromo.BrandTech != null ? newPromo.BrandTech.Name : null;
            OldPromoStatus = oldPromo.PromoStatus != null ? oldPromo.PromoStatus.Name : null;
            NewPromoStatus = newPromo.PromoStatus != null ? newPromo.PromoStatus.Name : null;
            OldMarsMechanic = oldPromo.MarsMechanic != null ? oldPromo.MarsMechanic.Name : null;
            NewMarsMechanic = newPromo.MarsMechanic != null ? newPromo.MarsMechanic.Name : null;
            OldPlanInstoreMechanic = oldPromo.PlanInstoreMechanic != null ? oldPromo.PlanInstoreMechanic.Name : null;
            NewPlanInstoreMechanic = newPromo.PlanInstoreMechanic != null ? newPromo.PlanInstoreMechanic.Name : null;
            OldMarsMechanicDiscount = oldPromo.MarsMechanicDiscount;
            NewMarsMechanicDiscount = newPromo.MarsMechanicDiscount;
            OldPlanInstoreMechanicDiscount = oldPromo.PlanInstoreMechanicDiscount;
            NewPlanInstoreMechanicDiscount = newPromo.PlanInstoreMechanicDiscount;
            OldDispatchesStart = oldPromo.DispatchesStart;
            NewDispatchesStart = newPromo.DispatchesStart;
            OldDispatchesEnd = oldPromo.DispatchesEnd;
            NewDispatchesEnd = newPromo.DispatchesEnd;
            OldStartDate = oldPromo.StartDate;
            NewStartDate = newPromo.StartDate;
            OldEndDate = oldPromo.EndDate;
            NewEndDate = newPromo.EndDate;
            OldPlanPromoUpliftPercent = oldPromo.PlanPromoUpliftPercent;
            NewPlanPromoUpliftPercent = newPromo.PlanPromoUpliftPercent;
            OldPlanPromoIncrementalLSV = oldPromo.PlanPromoIncrementalLSV;
            NewPlanPromoIncrementalLSV = newPromo.PlanPromoIncrementalLSV;
            IsCreate = false;
            IsDelete = false;
        }

        /// <summary>
        /// Конструктор для создания записи о создании промо
        /// </summary>
        /// <param name="promo"></param>
        /// <param name="isDelete"></param>
        public PromoDemandChangeIncident(Promo promo, bool isDelete = false) {
            PromoIntId = promo.Number;
            Name = promo.Name;
            ClientHierarchy = promo.ClientHierarchy;
            BrandTech = promo.BrandTech != null ? promo.BrandTech.Name : null;
            NewPromoStatus = promo.PromoStatus != null ? promo.PromoStatus.Name : null;
            OldMarsMechanic = promo.MarsMechanic != null ? promo.MarsMechanic.Name : null;
            NewMarsMechanic = promo.MarsMechanic != null ? promo.MarsMechanic.Name : null;
            OldPlanInstoreMechanic = promo.PlanInstoreMechanic != null ? promo.PlanInstoreMechanic.Name : null;
            NewPlanInstoreMechanic = promo.PlanInstoreMechanic != null ? promo.PlanInstoreMechanic.Name : null;
            OldMarsMechanicDiscount = promo.MarsMechanicDiscount;
            NewMarsMechanicDiscount = promo.MarsMechanicDiscount;
            OldPlanInstoreMechanicDiscount = promo.PlanInstoreMechanicDiscount;
            NewPlanInstoreMechanicDiscount = promo.PlanInstoreMechanicDiscount;
            OldDispatchesStart = promo.DispatchesStart;
            NewDispatchesStart = promo.DispatchesStart;
            OldDispatchesEnd = promo.DispatchesEnd;
            NewDispatchesEnd = promo.DispatchesEnd;
            OldStartDate = promo.StartDate;
            NewStartDate = promo.StartDate;
            OldEndDate = promo.EndDate;
            NewEndDate = promo.EndDate;
            OldPlanPromoUpliftPercent = promo.PlanPromoUpliftPercent;
            NewPlanPromoUpliftPercent = promo.PlanPromoUpliftPercent;
            OldPlanPromoIncrementalLSV = promo.PlanPromoIncrementalLSV;
            NewPlanPromoIncrementalLSV = promo.PlanPromoIncrementalLSV;
            IsCreate = !isDelete;
            IsDelete = isDelete;
        }
    }
}
