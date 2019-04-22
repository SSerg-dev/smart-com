using System;
using Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Persist;
using System.Linq;

namespace Module.Persist.TPM.Model.TPM
{
    public class Promo : IEntity<Guid>, IDeactivatable
    {
        [NotMapped]
        private Guid id;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                GetCalculationStatus();
            }
        }

        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? TechnologyId { get; set; }
        public Guid? BrandTechId { get; set; }
        public Guid? PromoStatusId { get; set; }
        public Guid? MarsMechanicId { get; set; }
        public Guid? MarsMechanicTypeId { get; set; }
        public Guid? PlanInstoreMechanicId { get; set; }
        public Guid? PlanInstoreMechanicTypeId { get; set; }
        public Guid? ColorId { get; set; }
        public Guid? RejectReasonId { get; set; }
        public Guid? EventId { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? ActualInStoreMechanicId { get; set; }
        public Guid? ActualInStoreMechanicTypeId { get; set; }
        public int? ClientTreeId { get; set; }
        public int? BaseClientTreeId { get; set; }
        [StringLength(400)]
        public string BaseClientTreeIds { get; set; }
        public bool? NeedRecountUplift { get; set; }

        public DateTimeOffset? LastApprovedDate { get; set; }

        // Basic
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Number { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string ClientHierarchy { get; set; }
        public string ProductHierarchy { get; set; }
        [StringLength(255)]
        public string MechanicComment { get; set; }
        public int? MarsMechanicDiscount { get; set; }
        public int? PlanInstoreMechanicDiscount { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }
        public string InvoiceNumber { get; set; }

        [StringLength(20)]
        public string Mechanic { get; set; }
        [StringLength(20)]
        public string MechanicIA { get; set; }

        //MarsDates
        [StringLength(15)]
        public string MarsStartDate { get; set; }
        [StringLength(15)]
        public string MarsEndDate { get; set; }
        [StringLength(15)]
        public string MarsDispatchesStart { get; set; }
        [StringLength(15)]
        public string MarsDispatchesEnd { get; set; }

        [StringLength(255)]
        public string OtherEventName { get; set; }
        public string EventName { get; set; }
        public int? CalendarPriority { get; set; }

        // Calculation
        public double? PlanPromoTIShopper { get; set; }
        public double? PlanPromoTIMarketing { get; set; }
        public double? PlanPromoBranding { get; set; }
        public double? PlanPromoCost { get; set; }
        public double? PlanPromoBTL { get; set; }
        public double? PlanPromoCostProduction { get; set; }
        public double? PlanPromoUpliftPercent { get; set; }
        public double? PlanPromoIncrementalLSV { get; set; }
        public double? PlanPromoLSV { get; set; }

        //необходимость полей в таком виде под вопросом
        public double? PlanPostPromoEffect { get; set; }
        public double? PlanPostPromoEffectW1 { get; set; }
        public double? PlanPostPromoEffectW2 { get; set; }
        //

        public int? PlanPromoROIPercent { get; set; }
        public double? PlanPromoIncrementalNSV { get; set; }
        public double? PlanPromoNetIncrementalNSV { get; set; }
        public double? PlanPromoIncrementalMAC { get; set; }
        public double? PlanPromoXSites { get; set; }
        public double? PlanPromoCatalogue { get; set; }
        public double? PlanPromoPOSMInClient { get; set; }
        public double? PlanPromoCostProdXSites { get; set; }
        public double? PlanPromoCostProdCatalogue { get; set; }
        public double? PlanPromoCostProdPOSMInClient { get; set; }
        public double? ActualPromoXSites { get; set; }
        public double? ActualPromoCatalogue { get; set; }
        public double? ActualPromoPOSMInClient { get; set; }
        public double? ActualPromoCostProdXSites { get; set; }
        public double? ActualPromoCostProdCatalogue { get; set; }
        public double? ActualPromoCostProdPOSMInClient { get; set; }
        public double? PlanPromoBaselineLSV { get; set; }
        public double? PlanPromoIncrementalBaseTI { get; set; }
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public int? PlanPromoNetROIPercent { get; set; }
        public int? PlanPromoNetUpliftPercent { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public int? ActualInStoreDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
        public double? ActualPromoTotalCost { get; set; }
        public double? ActualPromoNetIncrementalLSV { get; set; }
        public double? ActualPromoNetLSV { get; set; }
        public double? ActualPromoNetIncrementalMAC { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? ActualPromoNetIncrementalEarnings { get; set; }
        public int? ActualPromoNetROIPercent { get; set; }
        public int? ActualPromoNetUpliftPercent { get; set; }
        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoNetNSV { get; set; }
        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }

        // Promo Closure
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }
        public double? ActualPromoBranding { get; set; }
        public double? ActualPromoBTL { get; set; }
        public double? ActualPromoCostProduction { get; set; }
        public double? ActualPromoCost { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? ActualPromoLSV { get; set; }

        //необходимость полей в таком виде под вопросом
        public int? FactPostPromoEffect { get; set; }
        public double? FactPostPromoEffectW1 { get; set; }
        public double? FactPostPromoEffectW2 { get; set; }
        //

        public int? ActualPromoROIPercent { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalMAC { get; set; }

        // Promo Approved
        public bool? IsAutomaticallyApproved { get; set; }
        public bool? IsCustomerMarketingApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Technology Technology { get; set; }
        public virtual BrandTech BrandTech { get; set; }
        public virtual PromoStatus PromoStatus { get; set; }
        [ForeignKey("MarsMechanicId")]
        public virtual Mechanic MarsMechanic { get; set; }
        [ForeignKey("PlanInstoreMechanicId")]
        public virtual Mechanic PlanInstoreMechanic { get; set; }
        [ForeignKey("MarsMechanicTypeId")]
        public virtual MechanicType MarsMechanicType { get; set; }
        [ForeignKey("PlanInstoreMechanicTypeId")]
        public virtual MechanicType PlanInstoreMechanicType { get; set; }
        public virtual Color Color { get; set; }
        public virtual RejectReason RejectReason { get; set; }
        public virtual Event Event { get; set; }
        [ForeignKey("ActualInStoreMechanicId")]
        public virtual Mechanic ActualInStoreMechanic { get; set; }
        [ForeignKey("ActualInStoreMechanicTypeId")]
        public virtual MechanicType ActualInStoreMechanicType { get; set; }

        //Поля для отчёта ROIReport
        [StringLength(255)]
        public string Client1LevelName { get; set; }
        [StringLength(255)]
        public string Client2LevelName { get; set; }
        [StringLength(255)]
        public string ClientName { get; set; }
        [StringLength(500)]
        public string ProductSubrangesList { get; set; }

        // Not Mapped
        public string ProductTreeObjectIds { get; set; }

        /// <summary>
        /// Показывает, производится ли расчет по данному промо
        /// </summary>
        public bool? Calculating { get; set; }
        /// <summary>
        /// Информация какой обработчик блокировал промо и когда. Формат: HandlerId_BlockDateTime
        /// </summary>
        public string BlockInformation { get; set; }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="promoToCopy"></param>
        public Promo(Promo promoToCopy) {
            Name = promoToCopy.Name;
            Number = promoToCopy.Number;
            ClientHierarchy = promoToCopy.ClientHierarchy;
            ClientTreeId = promoToCopy.ClientTreeId;
            PromoStatus = promoToCopy.PromoStatus;
            BrandTech = promoToCopy.BrandTech;
            MarsMechanic = promoToCopy.MarsMechanic;
            MarsMechanicId = promoToCopy.MarsMechanicId;
            MarsMechanicTypeId = promoToCopy.MarsMechanicTypeId;
            MarsMechanicDiscount = promoToCopy.MarsMechanicDiscount;            
            PlanInstoreMechanic = promoToCopy.PlanInstoreMechanic;
            PlanInstoreMechanicId = promoToCopy.PlanInstoreMechanicId;
            PlanInstoreMechanicTypeId = promoToCopy.PlanInstoreMechanicTypeId;
            PlanInstoreMechanicDiscount = promoToCopy.PlanInstoreMechanicDiscount;
            StartDate = promoToCopy.StartDate;
            EndDate = promoToCopy.EndDate;
            DispatchesStart = promoToCopy.DispatchesStart;
            DispatchesEnd = promoToCopy.DispatchesEnd;
            PlanPromoUpliftPercent = promoToCopy.PlanPromoUpliftPercent;
            PlanPromoIncrementalLSV = promoToCopy.PlanPromoIncrementalLSV;
            ProductHierarchy = promoToCopy.ProductHierarchy;
        }

        public Promo() {}

        private void GetCalculationStatus()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    Calculating = context.Set<BlockedPromo>().Any(n => n.PromoId == id && !n.Disabled);
                }
            }
            catch { }
        }
    }
}
