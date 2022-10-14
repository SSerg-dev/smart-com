using Core.Data;
using Module.Persist.TPM.Model.Interfaces;
using Newtonsoft.Json;
using Persist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Module.Persist.TPM.Model.TPM
{
    public class Promo : IEntity<Guid>, IDeactivatable, IMode
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
                GetPromoBasicProducts();
            }
        }

        public bool Disabled { get; set; }
        [Index("Unique_PromoNumber", 3, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }
        [Index("Unique_PromoNumber", 2, IsUnique = true)]
        public TPMmode TPMmode { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public DateTimeOffset? LastChangedDateDemand { get; set; }
        public DateTimeOffset? LastChangedDateFinance { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public int? ClientTreeId { get; set; }

        public Guid? CreatorId { get; set; }
        public string CreatorLogin { get; set; }

        public int? BaseClientTreeId { get; set; }
        [StringLength(400)]
        public string BaseClientTreeIds { get; set; }
        public bool? NeedRecountUplift { get; set; }

        public DateTimeOffset? LastApprovedDate { get; set; }

        // Basic
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Index("Unique_PromoNumber", 1, IsUnique = true)]
        public int? Number { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string ClientHierarchy { get; set; }
        public string ProductHierarchy { get; set; }
        [StringLength(255)]
        public string MechanicComment { get; set; }
        public double? MarsMechanicDiscount { get; set; }
        public double? PlanInstoreMechanicDiscount { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? BudgetYear { get; set; }
        public int? PromoDuration { get; set; }
        public int? DispatchDuration { get; set; }
        public string InvoiceNumber { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsOnInvoice { get; set; }

        [StringLength(255)]
        public string Mechanic { get; set; }
        [StringLength(255)]
        public string MechanicIA { get; set; }
        public double DeviationCoefficient { get; set; }

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

        public double? PlanPromoPostPromoEffectLSVW1 { get; set; }
        public double? PlanPromoPostPromoEffectLSVW2 { get; set; }
        public double? PlanPromoPostPromoEffectLSV { get; set; }

        public double? PlanPromoROIPercent { get; set; }
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
        public double? PlanPromoNetIncrementalBaseTI { get; set; }
        public double? PlanPromoIncrementalCOGS { get; set; }
        public double? PlanPromoNetIncrementalCOGS { get; set; }
        public double? PlanPromoTotalCost { get; set; }
        public double? PlanPromoNetIncrementalLSV { get; set; }
        public double? PlanPromoNetLSV { get; set; }
        public double? PlanPromoNetIncrementalMAC { get; set; }
        public double? PlanPromoIncrementalEarnings { get; set; }
        public double? PlanPromoNetIncrementalEarnings { get; set; }
        public double? PlanPromoNetROIPercent { get; set; }
        public double? PlanPromoNetUpliftPercent { get; set; }
        public double? PlanTIBasePercent { get; set; }
        public double? PlanCOGSPercent { get; set; }
        public double? PlanCOGSTn { get; set; }
        public double? ActualPromoBaselineLSV { get; set; }
        public double? ActualInStoreDiscount { get; set; }
        public double? ActualInStoreShelfPrice { get; set; }
        public double? PlanInStoreShelfPrice { get; set; }
        public double? ActualPromoIncrementalBaseTI { get; set; }
        public double? ActualPromoNetIncrementalBaseTI { get; set; }
        public double? ActualPromoIncrementalCOGS { get; set; }
        public double? ActualPromoNetIncrementalCOGS { get; set; }
        public double? ActualPromoTotalCost { get; set; }
        public double? ActualPromoNetIncrementalLSV { get; set; }
        public double? ActualPromoNetLSV { get; set; }
        public double? ActualPromoNetIncrementalMAC { get; set; }
        public double? ActualPromoIncrementalEarnings { get; set; }
        public double? ActualPromoNetIncrementalEarnings { get; set; }
        public double? ActualPromoNetROIPercent { get; set; }
        public double? ActualPromoNetUpliftPercent { get; set; }
        public double? ActualTIBasePercent { get; set; }
        public double? ActualCOGSPercent { get; set; }
        public double? ActualCOGSTn { get; set; }
        public double? PlanPromoBaselineBaseTI { get; set; }
        public double? PlanPromoBaseTI { get; set; }
        public double? PlanPromoNetBaseTI { get; set; }
        public double? PlanPromoNSV { get; set; }
        public double? PlanPromoNetNSV { get; set; }
        public double? ActualPromoBaselineBaseTI { get; set; }
        public double? ActualPromoNetBaseTI { get; set; }
        public double? ActualPromoNSV { get; set; }
        public double? ActualPromoBaseTI { get; set; }
        public double? ActualPromoNetNSV { get; set; }
        public double? SumInvoice { get; set; }
        public bool? ManualInputSumInvoice { get; set; }
        // New ROI Calculation parameters
        public double? PlanPromoIncrementalMACLSV { get; set; }
        public double? PlanPromoNetIncrementalMACLSV { get; set; }
        public double? ActualPromoIncrementalMACLSV { get; set; }
        public double? ActualPromoNetIncrementalMACLSV { get; set; }
        public double? PlanPromoIncrementalEarningsLSV { get; set; }
        public double? PlanPromoNetIncrementalEarningsLSV { get; set; }
        public double? ActualPromoIncrementalEarningsLSV { get; set; }
        public double? ActualPromoNetIncrementalEarningsLSV { get; set; }
        public double? PlanPromoROIPercentLSV { get; set; }
        public double? PlanPromoNetROIPercentLSV { get; set; }
        public double? ActualPromoROIPercentLSV { get; set; }
        public double? ActualPromoNetROIPercentLSV { get; set; }

        // Add TI 
        public double? PlanAddTIShopperApproved { get; set; }
        public double? PlanAddTIShopperCalculated { get; set; }
        public double? PlanAddTIMarketingApproved { get; set; }
        public double? ActualAddTIShopper { get; set; }
        public double? ActualAddTIMarketing { get; set; }


        public bool UseActualTI { get; set; }
        public bool UseActualCOGS { get; set; }

        // Promo Closure
        public double? ActualPromoTIShopper { get; set; }
        public double? ActualPromoTIMarketing { get; set; }
        public double? ActualPromoBranding { get; set; }
        public double? ActualPromoBTL { get; set; }
        public double? ActualPromoCostProduction { get; set; }
        public double? ActualPromoCost { get; set; }
        public double? ActualPromoUpliftPercent { get; set; }
        public double? ActualPromoIncrementalLSV { get; set; }
        public double? ActualPromoLSVByCompensation { get; set; }
        public double? ActualPromoLSV { get; set; }
        public double? ActualPromoLSVSI { get; set; }
        public double? ActualPromoLSVSO { get; set; }

        public double? ActualPromoPostPromoEffectLSVW1 { get; set; }
        public double? ActualPromoPostPromoEffectLSVW2 { get; set; }
        public double? ActualPromoPostPromoEffectLSV { get; set; }

        public double? ActualPromoROIPercent { get; set; }
        public double? ActualPromoIncrementalNSV { get; set; }
        public double? ActualPromoNetIncrementalNSV { get; set; }
        public double? ActualPromoIncrementalMAC { get; set; }

        // Promo Approved
        public bool? IsAutomaticallyApproved { get; set; }
        public bool? IsCMManagerApproved { get; set; }
        public bool? IsDemandPlanningApproved { get; set; }
        public bool? IsDemandFinanceApproved { get; set; }
        public bool? IsGAManagerApproved { get; set; }

        [StringLength(500)]
        public string ProductSubrangesList { get; set; }
        [StringLength(500)]
        public string ProductSubrangesListRU { get; set; }

        // Not Mapped
        //[NotMapped]
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
        /// Список продуктов в формате JSON
        /// </summary>
        // Почему JSON? Привет ExtJS, Odata и Breeze за удобную работу с моделями
        public string PromoBasicProducts { get; set; }

        /// <summary>
        /// ID для обозначения операций над промо, позволяет избедать дубрирования в Raven
        /// </summary>
        [NotMapped] // не маппим в БД MS SQL
        public Guid OperationId { get; set; } = Guid.NewGuid(); // присваивание здесь позволяет не думать об этом :)

        public bool LoadFromTLC { get; set; }

        public bool? InOut { get; set; }
        public string InOutProductIds { get; set; }
        public bool InOutExcludeAssortmentMatrixProductsButtonPressed { get; set; }
        public string RegularExcludedProductIds { get; set; }

        [StringLength(100)]
        public string AdditionalUserTimestamp { get; set; }

        public bool IsGrowthAcceleration { get; set; }
        public bool IsApolloExport { get; set; }
        public bool IsInExchange { get; set; }
        public string LinkedPromoes { get; set; }
        public bool IsSplittable { get; set; }
        public bool IsLSVBased { get; set; }

        public double? PlanPromoBaselineVolume { get; set; }
        public double? PlanPromoPostPromoEffectVolume { get; set; }
        public double? PlanPromoPostPromoEffectVolumeW1 { get; set; }
        public double? PlanPromoPostPromoEffectVolumeW2 { get; set; }
        public double? PlanPromoIncrementalVolume { get; set; }
        public double? PlanPromoNetIncrementalVolume { get; set; }
        public double? ActualPromoBaselineVolume { get; set; }
        public double? ActualPromoPostPromoEffectVolume { get; set; }
        public double? ActualPromoVolumeByCompensation { get; set; }
        public double? ActualPromoVolumeSI { get; set; }
        public double? ActualPromoVolume { get; set; }
        public double? ActualPromoIncrementalVolume { get; set; }
        public double? ActualPromoNetIncrementalVolume { get; set; }
        public double? PlanPromoIncrementalCOGSTn { get; set; }
        public double? PlanPromoNetIncrementalCOGSTn { get; set; }
        public double? ActualPromoIncrementalCOGSTn { get; set; }
        public double? ActualPromoNetIncrementalCOGSTn { get; set; }

        // соединения к другим entity сначала с ключами, потом списки
        public Guid? BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public Guid? TechnologyId { get; set; }
        public virtual Technology Technology { get; set; }
        public Guid? BrandTechId { get; set; }
        public virtual BrandTech BrandTech { get; set; }
        public int? ClientTreeKeyId { get; set; }
        [ForeignKey("ClientTreeKeyId")]
        public virtual ClientTree ClientTree { get; set; }
        public Guid? PromoStatusId { get; set; }
        public virtual PromoStatus PromoStatus { get; set; }
        public Guid? MarsMechanicId { get; set; }
        [ForeignKey("MarsMechanicId")]
        public virtual Mechanic MarsMechanic { get; set; }
        public Guid? PlanInstoreMechanicId { get; set; }
        [ForeignKey("PlanInstoreMechanicId")]
        public virtual Mechanic PlanInstoreMechanic { get; set; }
        public Guid? MarsMechanicTypeId { get; set; }
        [ForeignKey("MarsMechanicTypeId")]
        public virtual MechanicType MarsMechanicType { get; set; }
        public Guid? PlanInstoreMechanicTypeId { get; set; }
        [ForeignKey("PlanInstoreMechanicTypeId")]
        public virtual MechanicType PlanInstoreMechanicType { get; set; }
        public Guid? PromoTypesId { get; set; }
        [ForeignKey("PromoTypesId")]
        public virtual PromoTypes PromoTypes { get; set; }
        public Guid? ColorId { get; set; }
        public virtual Color Color { get; set; }
        public Guid? RejectReasonId { get; set; }
        public virtual RejectReason RejectReason { get; set; }
        public Guid? EventId { get; set; }
        public virtual Event Event { get; set; }
        public Guid? ActualInStoreMechanicId { get; set; }
        [ForeignKey("ActualInStoreMechanicId")]
        public virtual Mechanic ActualInStoreMechanic { get; set; }
        public Guid? ActualInStoreMechanicTypeId { get; set; }
        [ForeignKey("ActualInStoreMechanicTypeId")]
        public virtual MechanicType ActualInStoreMechanicType { get; set; }
        [ForeignKey("MasterPromo")]
        public Guid? MasterPromoId { get; set; }
        public virtual Promo MasterPromo { get; set; }
        [ForeignKey("RollingScenario")]
        public Guid? RollingScenarioId { get; set; }
        public RollingScenario RollingScenario { get; set; }

        public virtual ICollection<Promo> Promoes { get; set; }
        public ICollection<PromoProduct> PromoProducts { get; set; }
        public ICollection<IncrementalPromo> IncrementalPromoes { get; set; }
        public ICollection<PreviousDayIncremental> PreviousDayIncrementals { get; set; }
        public ICollection<PromoProductTree> PromoProductTrees { get; set; }
        public ICollection<PromoStatusChange> PromoStatusChanges { get; set; }
        public ICollection<PromoSupportPromo> PromoSupportPromoes { get; set; }
        public ICollection<PromoUpliftFailIncident> PromoUpliftFailIncidents { get; set; }
        public ICollection<BTLPromo> BTLPromoes { get; set; }
        public ICollection<PromoOnApprovalIncident> PromoOnApprovalIncidents { get; set; }
        public ICollection<PromoOnRejectIncident> PromoOnRejectIncidents { get; set; }
        public ICollection<PromoCancelledIncident> PromoCancelledIncidents { get; set; }
        public ICollection<PromoApprovedIncident> PromoApprovedIncidents { get; set; }
        public ICollection<CurrentDayIncremental> CurrentDayIncrementals { get; set; }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="promoToCopy"></param>
        //public Promo(Promo promoToCopy)
        //{
        //    Name = promoToCopy.Name;
        //    Number = promoToCopy.Number;
        //    ClientHierarchy = promoToCopy.ClientHierarchy;
        //    ClientTreeId = promoToCopy.ClientTreeId;
        //    PromoStatus = promoToCopy.PromoStatus;
        //    BrandTech = promoToCopy.BrandTech;
        //    MarsMechanic = promoToCopy.MarsMechanic;
        //    MarsMechanicId = promoToCopy.MarsMechanicId;
        //    MarsMechanicTypeId = promoToCopy.MarsMechanicTypeId;
        //    MarsMechanicDiscount = promoToCopy.MarsMechanicDiscount;
        //    PlanInstoreMechanic = promoToCopy.PlanInstoreMechanic;
        //    PlanInstoreMechanicId = promoToCopy.PlanInstoreMechanicId;
        //    PlanInstoreMechanicTypeId = promoToCopy.PlanInstoreMechanicTypeId;
        //    PlanInstoreMechanicDiscount = promoToCopy.PlanInstoreMechanicDiscount;
        //    StartDate = promoToCopy.StartDate;
        //    EndDate = promoToCopy.EndDate;
        //    DispatchesStart = promoToCopy.DispatchesStart;
        //    DispatchesEnd = promoToCopy.DispatchesEnd;
        //    BudgetYear = promoToCopy.BudgetYear;
        //    PlanPromoUpliftPercent = promoToCopy.PlanPromoUpliftPercent;
        //    PlanPromoIncrementalLSV = promoToCopy.PlanPromoIncrementalLSV;
        //    ProductHierarchy = promoToCopy.ProductHierarchy;
        //    PromoStatusId = promoToCopy.PromoStatusId;
        //    NeedRecountUplift = promoToCopy.NeedRecountUplift;
        //    InOut = promoToCopy.InOut;
        //    InOutProductIds = promoToCopy.InOutProductIds;
        //    InOutExcludeAssortmentMatrixProductsButtonPressed = promoToCopy.InOutExcludeAssortmentMatrixProductsButtonPressed;
        //    IsDemandPlanningApproved = promoToCopy.IsDemandPlanningApproved;
        //    IsDemandFinanceApproved = promoToCopy.IsDemandFinanceApproved;
        //    IsCMManagerApproved = promoToCopy.IsCMManagerApproved;
        //    ActualPromoUpliftPercent = promoToCopy.ActualPromoUpliftPercent;
        //    ActualPromoIncrementalBaseTI = promoToCopy.ActualPromoIncrementalBaseTI;
        //    ActualPromoIncrementalCOGS = promoToCopy.ActualPromoIncrementalCOGS;
        //    ActualPromoIncrementalEarnings = promoToCopy.ActualPromoIncrementalEarnings;
        //    ActualPromoIncrementalLSV = promoToCopy.ActualPromoIncrementalLSV;
        //    ActualPromoIncrementalMAC = promoToCopy.ActualPromoIncrementalMAC;
        //    ActualPromoIncrementalMACLSV = promoToCopy.ActualPromoIncrementalMACLSV;
        //    ActualPromoIncrementalNSV = promoToCopy.ActualPromoIncrementalNSV;
        //    ActualPromoLSV = promoToCopy.ActualPromoLSV;
        //    ActualPromoLSVSI = promoToCopy.ActualPromoLSVSI;
        //    ActualPromoLSVSO = promoToCopy.ActualPromoLSVSO;
        //    ActualPromoLSVByCompensation = promoToCopy.ActualPromoLSVByCompensation;
        //    PlanPromoUpliftPercent = promoToCopy.PlanPromoUpliftPercent;
        //    PlanPromoIncrementalBaseTI = promoToCopy.PlanPromoIncrementalBaseTI;
        //    PlanPromoIncrementalCOGS = promoToCopy.PlanPromoIncrementalCOGS;
        //    PlanPromoIncrementalEarnings = promoToCopy.PlanPromoIncrementalEarnings;
        //    PlanPromoIncrementalLSV = promoToCopy.PlanPromoIncrementalLSV;
        //    PlanPromoIncrementalMAC = promoToCopy.PlanPromoIncrementalMAC;
        //    PlanPromoIncrementalMACLSV = promoToCopy.PlanPromoIncrementalMACLSV;
        //    PlanPromoIncrementalNSV = promoToCopy.PlanPromoIncrementalNSV;
        //    ActualAddTIMarketing = promoToCopy.ActualAddTIMarketing;
        //    PlanAddTIMarketingApproved = promoToCopy.PlanAddTIMarketingApproved;
        //    ActualAddTIShopper = promoToCopy.ActualAddTIShopper;
        //    PlanAddTIShopperApproved = promoToCopy.PlanAddTIShopperApproved;
        //    PlanAddTIShopperCalculated = promoToCopy.PlanAddTIShopperCalculated;
        //    PlanPromoLSV = promoToCopy.PlanPromoLSV;
        //    EventId = promoToCopy.EventId;
        //    CalendarPriority = promoToCopy.CalendarPriority;
        //    RegularExcludedProductIds = promoToCopy.RegularExcludedProductIds;
        //    IsGrowthAcceleration = promoToCopy.IsGrowthAcceleration;
        //    PromoTypesId = promoToCopy.PromoTypesId;
        //    PlanTIBasePercent = promoToCopy.PlanTIBasePercent;
        //    ActualTIBasePercent = promoToCopy.ActualTIBasePercent;
        //    PlanCOGSPercent = promoToCopy.PlanCOGSPercent;
        //    ActualCOGSPercent = promoToCopy.ActualCOGSPercent;
        //    PlanCOGSTn = promoToCopy.PlanCOGSTn;
        //    ActualCOGSTn = promoToCopy.ActualCOGSTn;
        //    IsOnInvoice = promoToCopy.IsOnInvoice;
        //    IsApolloExport = promoToCopy.IsApolloExport;
        //    ManualInputSumInvoice = promoToCopy.ManualInputSumInvoice;
        //    IsInExchange = promoToCopy.IsInExchange;
        //    MasterPromoId = promoToCopy.MasterPromoId;
        //    TPMmode = promoToCopy.TPMmode;
        //}

        //public Promo() { }

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

        /// <summary>
        /// Поиск сведений о выбранных узлах в дереве продуктов
        /// </summary>
        private void GetPromoBasicProducts()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    // Ищем в таблице, обеспечивающей связь М-М, затем в таблице продуктового дерева
                    int[] productObjectIds = context.Set<PromoProductTree>().Where(n => n.PromoId == id && !n.Disabled).Select(n => n.ProductTreeObjectId).ToArray();
                    ProductTree[] products = context.Set<ProductTree>().Where(n => productObjectIds.Contains(n.ObjectId) && !n.EndDate.HasValue).ToArray();

                    if (products.Length > 0)
                    {
                        PromoBasicProduct promoBasicProducts = new PromoBasicProduct
                        {
                            // выбранные узлы
                            ProductsChoosen = products.Select(n => new
                            {
                                n.ObjectId,
                                n.Name,
                                n.Type,
                                n.FullPathName,
                                n.Abbreviation,
                                n.LogoFileName,
                                n.Filter
                            }).ToArray()
                        };

                        // формируем название Brand и Technology
                        ProductTree currentNode = products[0];
                        while (currentNode != null && currentNode.Type.IndexOf("root") < 0)
                        {
                            if (currentNode.Type.IndexOf("Brand") >= 0)
                            {
                                promoBasicProducts.Brand = currentNode.Name;
                                promoBasicProducts.BrandAbbreviation = currentNode.Abbreviation;

                                // если есть технология, то и логотип уже есть
                                if (promoBasicProducts.LogoFileName == null)
                                    promoBasicProducts.LogoFileName = currentNode.LogoFileName;
                            }
                            else if (currentNode.Type.IndexOf("Technology") >= 0)
                            {
                                promoBasicProducts.Technology = currentNode.Name;
                                promoBasicProducts.TechnologyAbbreviation = currentNode.Abbreviation;
                                promoBasicProducts.LogoFileName = currentNode.LogoFileName;
                            }

                            currentNode = context.Set<ProductTree>().FirstOrDefault(n => n.ObjectId == currentNode.parentId && !n.EndDate.HasValue);
                        }

                        PromoBasicProducts = JsonConvert.SerializeObject(promoBasicProducts, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }
                }
            }
            catch { }
        }
    }

    // Класс обертка для формы выбора продуктов в форме PROMO
    // Экономит память, дает обойтись малой кровью, позвляет не делать лишний запрос
    public class PromoBasicProduct
    {
        public string Brand { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Technology { get; set; }
        public string TechnologyAbbreviation { get; set; }
        public string LogoFileName { get; set; }
        public object[] ProductsChoosen { get; set; }
    }
}
