using Core.History;
using Core.ModuleRegistrator;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.History;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Http.OData.Builder;

namespace Module.Persist.TPM
{
    public class PersistTPMModuleRegistrator : IPersistModuleRegistrator
    {
        public void ConfigurateDBModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Brand>();
            modelBuilder.Entity<NonPromoEquipment>();
            modelBuilder.Entity<Segment>();
            modelBuilder.Entity<Technology>();
            modelBuilder.Entity<TechHighLevel>();
            modelBuilder.Entity<Program>();
            modelBuilder.Entity<Format>();
            modelBuilder.Entity<BrandTech>().HasRequired(g => g.Brand).WithMany(g => g.BrandTeches);
            modelBuilder.Entity<BrandTech>().HasRequired(g => g.Technology).WithMany(g => g.BrandTeches);
            modelBuilder.Entity<Subrange>();
            modelBuilder.Entity<AgeGroup>();
            modelBuilder.Entity<Variety>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Region>();
            modelBuilder.Entity<CommercialNet>();
            modelBuilder.Entity<CommercialSubnet>();
            modelBuilder.Entity<Distributor>();
            modelBuilder.Entity<StoreType>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Mechanic>();
            modelBuilder.Entity<MechanicType>();
            modelBuilder.Entity<PromoStatus>();
            modelBuilder.Entity<Budget>();
            modelBuilder.Entity<BudgetItem>();
            modelBuilder.Entity<Promo>().HasMany(p => p.Promoes);
            modelBuilder.Entity<Promo>().HasOptional(p => p.MasterPromo);
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoProducts).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.BTLPromoes).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoSupportPromoes).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.IncrementalPromoes).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoProductTrees).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoUpliftFailIncidents).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PreviousDayIncrementals).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.CurrentDayIncrementals).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoStatusChanges).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoOnApprovalIncidents).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoOnRejectIncidents).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoCancelledIncidents).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Promo>().HasMany(p => p.PromoApprovedIncidents).WithRequired(g => g.Promo).WillCascadeOnDelete();
            modelBuilder.Entity<Sale>();
            modelBuilder.Entity<Color>();
            modelBuilder.Entity<PromoSales>();
            modelBuilder.Entity<Demand>();
            modelBuilder.Entity<RejectReason>();
            modelBuilder.Entity<ClientTree>();
            modelBuilder.Entity<ProductTree>();
            modelBuilder.Entity<Event>().HasMany(e => e.BTLs).WithRequired(e => e.Event);
            modelBuilder.Entity<EventType>().HasMany(e => e.Events).WithRequired(e => e.EventType);
            modelBuilder.Entity<NodeType>();
            modelBuilder.Entity<PromoStatusChange>();
            modelBuilder.Entity<PromoDemand>();
            modelBuilder.Entity<BaseClientTreeView>().ToTable("BaseClientTreeView");
            modelBuilder.Entity<RetailType>();
            modelBuilder.Entity<NoneNego>();
            modelBuilder.Entity<ExportQuery>();


            modelBuilder.Entity<Plu>();
            modelBuilder.Entity<AssortmentMatrix2Plu>();
            modelBuilder.Entity<PromoProduct2Plu>();
            modelBuilder.Entity<PLUDictionary>();

            modelBuilder.Entity<PromoProduct>().HasMany(p => p.PromoProductsCorrections).WithRequired(p => p.PromoProduct).WillCascadeOnDelete();
            modelBuilder.Entity<PromoProduct>().HasOptional(x => x.Plu).WithRequired();


            modelBuilder.Entity<BaseLine>().HasRequired(g => g.Product).WithMany(f => f.BaseLines);
            modelBuilder.Entity<IncreaseBaseLine>().HasRequired(g => g.Product).WithMany(f => f.IncreaseBaseLines);
            modelBuilder.Entity<ClientTreeSharesView>().ToTable("ClientTreeSharesView");
            modelBuilder.Entity<ProductChangeIncident>();
            modelBuilder.Entity<ClientTreeHierarchyView>();
            modelBuilder.Entity<ProductTreeHierarchyView>();
            modelBuilder.Entity<PromoDemandChangeIncident>();
            modelBuilder.Entity<BudgetSubItem>();
            modelBuilder.Entity<PromoSupport>().HasMany(p => p.PromoSupportPromo)
                .WithRequired(p => p.PromoSupport);
            modelBuilder.Entity<NonPromoSupport>();
            modelBuilder.Entity<ServiceInfo>();
            modelBuilder.Entity<NonPromoSupportBrandTech>();
            modelBuilder.Entity<PostPromoEffect>();
            modelBuilder.Entity<PromoSupportPromo>();
            modelBuilder.Entity<PromoProductTree>();
            modelBuilder.Entity<COGS>();
            modelBuilder.Entity<PlanCOGSTn>();
            modelBuilder.Entity<TradeInvestment>();
            modelBuilder.Entity<PromoUpliftFailIncident>();
            modelBuilder.Entity<BlockedPromo>();

            modelBuilder.Entity<AssortmentMatrix>().HasRequired(x => x.ClientTree).WithMany(d => d.AssortmentMatrices);
            modelBuilder.Entity<AssortmentMatrix>().HasRequired(x => x.Product).WithMany(d => d.AssortmentMatrices);
            modelBuilder.Entity<AssortmentMatrix>().HasOptional(x => x.Plu).WithRequired();

            modelBuilder.Entity<IncrementalPromo>();
            modelBuilder.Entity<PromoView>();
            modelBuilder.Entity<PromoRSView>();
            modelBuilder.Entity<PromoGridView>();
            modelBuilder.Entity<PlanIncrementalReport>();
            modelBuilder.Entity<PromoCancelledIncident>();
            modelBuilder.Entity<ChangesIncident>();
            modelBuilder.Entity<PromoOnApprovalIncident>();
            modelBuilder.Entity<PromoOnRejectIncident>();
            modelBuilder.Entity<EventClientTree>();
            modelBuilder.Entity<BudgetSubItemClientTree>();
            modelBuilder.Entity<PromoApprovedIncident>();
            modelBuilder.Entity<ClientTreeBrandTech>();
            modelBuilder.Entity<ClientTreeNeedUpdateIncident>();
            modelBuilder.Entity<PromoProductsCorrection>();
            modelBuilder.Entity<PreviousDayIncremental>();
            modelBuilder.Entity<CurrentDayIncremental>();
            modelBuilder.Entity<PromoProductsView>().ToTable("PromoProductsView");
            modelBuilder.Entity<PromoTypes>();
            modelBuilder.Entity<ActualCOGS>();
            modelBuilder.Entity<ActualCOGSTn>();
            modelBuilder.Entity<RATIShopper>();
            modelBuilder.Entity<ActualTradeInvestment>();
            modelBuilder.Entity<BTL>().HasMany(p => p.BTLPromoes)
                .WithRequired(p => p.BTL);
            modelBuilder.Entity<BTL>().HasRequired(p => p.Event).WithMany(g => g.BTLs);
            modelBuilder.Entity<BTLPromo>();
            modelBuilder.Entity<ClientDashboard>();
            modelBuilder.Entity<ClientDashboardView>().ToTable("ClientDashboardView");
            modelBuilder.Entity<ClientDashboardRSView>().ToTable("ClientDashboardRSView");
            modelBuilder.Entity<PriceList>();
            modelBuilder.Entity<CoefficientSI2SO>();
            modelBuilder.Entity<PromoProductDifference>();
            modelBuilder.Entity<RollingVolume>();
            modelBuilder.Entity<PlanPostPromoEffectReportWeekView>().ToTable("PlanPostPromoEffectReportWeekView");
            modelBuilder.Entity<PromoROIReport>().ToTable("PromoROIReportView");
            modelBuilder.Entity<PromoPriceIncreaseROIReport>().ToTable("PromoPriceIncreaseROIReportView");
            modelBuilder.Entity<NonPromoSupportDMP>();
            modelBuilder.Entity<PromoSupportDMP>();

            modelBuilder.Entity<InputBaseline>();

            modelBuilder.Entity<UserTokenCache>();

            modelBuilder.Entity<Promo>().Ignore(n => n.ProductTreeObjectIds);
            modelBuilder.Entity<Promo>().Ignore(n => n.Calculating);
            modelBuilder.Entity<Promo>().Ignore(n => n.PromoBasicProducts);

            // Calendar Competitor Entities
            modelBuilder.Entity<Competitor>();
            modelBuilder.Entity<CompetitorPromo>();
            modelBuilder.Entity<CompetitorBrandTech>();
            modelBuilder.Entity<RPASetting>();
            modelBuilder.Entity<RPA>();

            modelBuilder.Entity<RollingScenario>();
            modelBuilder.Entity<PromoProductCorrectionView>();

            modelBuilder.Entity<PromoPriceIncrease>().HasRequired(g => g.Promo).WithOptional(g => g.PromoPriceIncrease).WillCascadeOnDelete();
            modelBuilder.Entity<PromoProductPriceIncrease>().HasOptional(g => g.PromoProduct).WithMany(g => g.PromoProductPriceIncreases);
            modelBuilder.Entity<PromoProductPriceIncrease>().HasRequired(g => g.PromoPriceIncrease).WithMany(g => g.PromoProductPriceIncreases).WillCascadeOnDelete();
            modelBuilder.Entity<PromoProductCorrectionPriceIncrease>().HasRequired(g => g.PromoProductPriceIncrease).WithMany(g => g.ProductCorrectionPriceIncreases).WillCascadeOnDelete();
            modelBuilder.Entity<PromoProductPriceIncreasesView>().ToTable("PromoProductPriceIncreasesView");
            modelBuilder.Entity<PromoProductCorrectionPriceIncreaseView>().ToTable("PromoProductCorrectionPriceIncreaseView");

            modelBuilder.Entity<MetricsLiveHistory>().ToTable("MetricsLiveHistories");
            modelBuilder.Entity<CloudTask>();
            modelBuilder.Entity<DiscountRange>();
            modelBuilder.Entity<DurationRange>();
            modelBuilder.Entity<PlanPostPromoEffect>().HasRequired(g => g.DiscountRange);
            modelBuilder.Entity<PlanPostPromoEffect>().HasRequired(g => g.DurationRange);
            modelBuilder.Entity<PlanPostPromoEffect>().HasRequired(g => g.BrandTech);
            modelBuilder.Entity<PlanPostPromoEffect>().HasRequired(g => g.ClientTree);

            modelBuilder.Entity<SavedScenario>().HasRequired(g => g.RollingScenario);
            modelBuilder.Entity<SavedScenario>().HasMany(g => g.Promoes).WithOptional(g => g.SavedScenario);
            modelBuilder.Entity<SavedPromo>().HasMany(g => g.Promoes).WithOptional(g => g.SavedPromo).WillCascadeOnDelete();

            modelBuilder.Entity<TLCImport>().ToTable("TLCImports");
            modelBuilder.Entity<SavedSetting>().ToTable("SavedSettings");
        }



        public void BuildEdm(ODataConventionModelBuilder builder)
        {

            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Category>("DeletedCategories");
            builder.EntitySet<HistoricalCategory>("HistoricalCategories");
            builder.Entity<Category>().Collection.Action("ExportXLSX");
            builder.Entity<Category>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Category>("Categories");
            builder.Entity<HistoricalCategory>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCategory>("HistoricalCategories");

            builder.EntitySet<Brand>("Brands").HasManyBinding(g => g.BrandTeches, "BrandTeches");
            builder.EntitySet<Brand>("DeletedBrands").HasManyBinding(g => g.BrandTeches, "BrandTeches");
            builder.EntitySet<HistoricalBrand>("HistoricalBrands");
            builder.Entity<Brand>().Collection.Action("ExportXLSX");
            builder.Entity<Brand>().Collection.Action("FullImportXLSX");
            builder.Entity<Brand>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Brand>("Brands");
            builder.Entity<HistoricalBrand>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBrand>("HistoricalBrands");

            builder.EntitySet<NonPromoEquipment>("NonPromoEquipments").HasManyBinding(g => g.NonPromoSupports, "NonPromoSupports");
            builder.EntitySet<NonPromoEquipment>("NonPromoEquipments").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<NonPromoEquipment>("DeletedNonPromoEquipments").HasManyBinding(g => g.NonPromoSupports, "NonPromoSupports");
            builder.EntitySet<NonPromoEquipment>("DeletedNonPromoEquipments").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<HistoricalNonPromoEquipment>("HistoricalNonPromoEquipments");
            builder.Entity<NonPromoEquipment>().Collection.Action("ExportXLSX");
            builder.Entity<NonPromoEquipment>().Collection.Action("FullImportXLSX");
            builder.Entity<NonPromoEquipment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<NonPromoEquipment>("NonPromoEquipments");
            builder.Entity<HistoricalNonPromoEquipment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalNonPromoEquipment>("HistoricalNonPromoEquipments");

            builder.EntitySet<Segment>("Segments");
            builder.EntitySet<Segment>("DeletedSegments");
            builder.EntitySet<HistoricalSegment>("HistoricalSegments");
            builder.Entity<Segment>().Collection.Action("ExportXLSX");
            builder.Entity<Segment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Segment>("Segments");
            builder.Entity<HistoricalSegment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalSegment>("HistoricalSegments");

            builder.EntitySet<Event>("Events");
            builder.EntitySet<Event>("Events").HasRequiredBinding(e => e.EventType, "EventTypes");
            builder.EntitySet<Event>("Events").HasManyBinding(e => e.BTLs, "BTLs");
            builder.EntitySet<Event>("Events").HasManyBinding(g => g.EventClientTrees, "EventClientTrees");
            builder.EntitySet<Event>("DeletedEvents");
            builder.EntitySet<Event>("DeletedEvents").HasManyBinding(g => g.EventClientTrees, "EventClientTrees");
            builder.EntitySet<Event>("DeletedEvents").HasRequiredBinding(e => e.EventType, "EventTypes");
            builder.EntitySet<Event>("DeletedEvents").HasManyBinding(e => e.BTLs, "BTLs");
            builder.EntitySet<HistoricalEvent>("HistoricalEvents");
            builder.Entity<Event>().Collection.Action("ExportXLSX");
            builder.Entity<Event>().Collection.Action("FullImportXLSX");
            builder.Entity<Event>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Event>("Events");
            builder.Entity<HistoricalEvent>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalEvent>("HistoricalEvents");

            builder.EntitySet<EventType>("EventTypes");
            builder.EntitySet<EventType>("EventTypes").HasManyBinding(e => e.Events, "Events");
            builder.EntitySet<EventType>("DeletedEventTypes");
            builder.EntitySet<EventType>("DeletedEventTypes").HasManyBinding(e => e.Events, "Events");

            builder.EntitySet<EventClientTree>("EventClientTrees");
            builder.EntitySet<EventClientTree>("EventClientTrees").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<EventClientTree>("EventClientTrees").HasRequiredBinding(e => e.Event, "Events");
            builder.Entity<EventClientTree>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<EventClientTree>("EventClientTrees");

            builder.EntitySet<Technology>("Technologies").HasManyBinding(g => g.BrandTeches, "BrandTeches");
            builder.EntitySet<Technology>("DeletedTechnologies").HasManyBinding(g => g.BrandTeches, "BrandTeches");
            builder.EntitySet<Technology>("Technologies").HasManyBinding(g => g.ProductTrees, "ProductTrees");
            builder.EntitySet<Technology>("DeletedTechnologies").HasManyBinding(g => g.ProductTrees, "ProductTrees");
            builder.EntitySet<HistoricalTechnology>("HistoricalTechnologies");
            builder.Entity<Technology>().Collection.Action("ExportXLSX");
            builder.Entity<Technology>().Collection.Action("FullImportXLSX");
            builder.Entity<Technology>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Technology>("Technologies");
            builder.Entity<HistoricalTechnology>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalTechnology>("HistoricalTechnologies");

            builder.EntitySet<TechHighLevel>("TechHighLevels");
            builder.EntitySet<TechHighLevel>("DeletedTechHighLevels");
            builder.EntitySet<HistoricalTechHighLevel>("HistoricalTechHighLevels");
            builder.Entity<TechHighLevel>().Collection.Action("ExportXLSX");
            builder.Entity<TechHighLevel>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<TechHighLevel>("TechHighLevels");
            builder.Entity<HistoricalTechHighLevel>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalTechHighLevel>("HistoricalTechHighLevels");

            builder.EntitySet<Program>("Programs");
            builder.EntitySet<Program>("DeletedPrograms");
            builder.EntitySet<HistoricalProgram>("HistoricalPrograms");
            builder.Entity<Program>().Collection.Action("ExportXLSX");
            builder.Entity<Program>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Program>("Programs");
            builder.Entity<HistoricalProgram>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalProgram>("HistoricalPrograms");

            builder.EntitySet<Format>("Formats");
            builder.EntitySet<Format>("DeletedFormats");
            builder.EntitySet<HistoricalFormat>("HistoricalFormats");
            builder.Entity<Format>().Collection.Action("ExportXLSX");
            builder.Entity<Format>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Format>("Formats");
            builder.Entity<HistoricalFormat>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalFormat>("HistoricalFormats");

            builder.EntitySet<PromoTypes>("PromoTypes").HasManyBinding(g => g.Mechanics, "Mechanics");
            builder.EntitySet<PromoTypes>("DeletedPromoTypes").HasManyBinding(g => g.Mechanics, "Mechanics");
            builder.EntitySet<HistoricalPromoTypes>("HistoricalPromoTypes");
            builder.Entity<PromoTypes>().Collection.Action("ExportXLSX");
            builder.Entity<PromoTypes>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoTypes>("PromoTypes");
            builder.Entity<HistoricalPromoTypes>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoTypes>("HistoricalPromoTypes");


            builder.EntitySet<BrandTech>("BrandTeches").HasManyBinding(g => g.ClientTreeBrandTeches, "ClientTreeBrandTeches");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasManyBinding(g => g.ClientTreeBrandTeches, "ClientTreeBrandTeches");
            builder.EntitySet<BrandTech>("BrandTeches").HasManyBinding(g => g.CoefficientSI2SOs, "CoefficientSI2SOs");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasManyBinding(g => g.CoefficientSI2SOs, "CoefficientSI2SOs");
            builder.EntitySet<BrandTech>("BrandTeches").HasManyBinding(g => g.NonPromoSupportBrandTeches, "NonPromoSupportBrandTeches");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasManyBinding(g => g.NonPromoSupportBrandTeches, "NonPromoSupportBrandTeches");
            builder.EntitySet<BrandTech>("BrandTeches").HasManyBinding(g => g.TradeInvestments, "TradeInvestments");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasManyBinding(g => g.TradeInvestments, "TradeInvestments");
            builder.EntitySet<BrandTech>("BrandTeches").HasManyBinding(g => g.PlanPostPromoEffects, "PlanPostPromoEffects");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasManyBinding(g => g.PlanPostPromoEffects, "PlanPostPromoEffects");
            builder.EntitySet<HistoricalBrandTech>("HistoricalBrandTeches");
            builder.EntitySet<BrandTech>("BrandTeches").HasRequiredBinding(e => e.Brand, "Brands");
            builder.EntitySet<BrandTech>("BrandTeches").HasRequiredBinding(e => e.Technology, "Technologies");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasRequiredBinding(e => e.Brand, "Brands");
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasRequiredBinding(e => e.Technology, "Technologies");
            builder.Entity<BrandTech>().HasRequired(n => n.Brand, (n, b) => n.BrandId == b.Id);
            builder.Entity<BrandTech>().HasRequired(n => n.Technology, (n, t) => n.TechnologyId == t.Id);
            builder.Entity<BrandTech>().Collection.Action("ExportXLSX");
            builder.Entity<BrandTech>().Collection.Action("FullImportXLSX");
            builder.Entity<BrandTech>().Collection.Action("GetBrandTechById").CollectionParameter<string>("id");
            builder.Entity<BrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BrandTech>("BrandTeches");
            builder.Entity<HistoricalBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBrandTech>("HistoricalBrandTechs");

            builder.EntitySet<Subrange>("Subranges");
            builder.EntitySet<Subrange>("DeletedSubranges");
            builder.EntitySet<HistoricalSubrange>("HistoricalSubranges");
            builder.Entity<Subrange>().Collection.Action("ExportXLSX");
            builder.Entity<Subrange>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Subrange>("Subranges");
            builder.Entity<HistoricalSubrange>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalSubrange>("HistoricalSubranges");

            builder.EntitySet<AgeGroup>("AgeGroups");
            builder.EntitySet<AgeGroup>("DeletedAgeGroups");
            builder.EntitySet<HistoricalAgeGroup>("HistoricalAgeGroups");
            builder.Entity<AgeGroup>().Collection.Action("ExportXLSX");
            builder.Entity<AgeGroup>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<AgeGroup>("AgeGroups");
            builder.Entity<HistoricalAgeGroup>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalAgeGroup>("HistoricalAgeGroups");

            builder.EntitySet<Variety>("Varieties");
            builder.EntitySet<Variety>("DeletedVarieties");
            builder.EntitySet<HistoricalVariety>("HistoricalVarieties");
            builder.Entity<Variety>().Collection.Action("ExportXLSX");
            builder.Entity<Variety>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Variety>("Varieties");
            builder.Entity<HistoricalVariety>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalVariety>("HistoricalVarieties");

            builder.EntitySet<Mechanic>("Mechanics").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<Mechanic>("DeletedMechanics").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<HistoricalMechanic>("HistoricalMechanics");
            builder.Entity<Mechanic>().Collection.Action("ExportXLSX");
            builder.Entity<Mechanic>().Collection.Action("FullImportXLSX");
            builder.EntitySet<Mechanic>("Mechanics").HasRequiredBinding(e => e.PromoTypes, "PromoTypes");
            builder.EntitySet<Mechanic>("DeletedMechanics").HasRequiredBinding(e => e.PromoTypes, "PromoTypes");
            builder.Entity<Mechanic>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Mechanic>("Mechanics");
            builder.Entity<HistoricalMechanic>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalMechanic>("HistoricalMechanics");

            builder.EntitySet<MechanicType>("MechanicTypes").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<MechanicType>("MechanicTypes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<MechanicType>("DeletedMechanicTypes").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<MechanicType>("DeletedMechanicTypes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<HistoricalMechanicType>("HistoricalMechanicTypes");
            builder.Entity<MechanicType>().Collection.Action("ExportXLSX");
            builder.Entity<MechanicType>().Collection.Action("FullImportXLSX");
            builder.Entity<MechanicType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<MechanicType>("MechanicTypes");
            builder.Entity<HistoricalMechanicType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalMechanicType>("HistoricalMechanicTypes");

            builder.EntitySet<PromoStatus>("PromoStatuss").HasManyBinding(g => g.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<PromoStatus>("DeletedPromoStatuss").HasManyBinding(g => g.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<HistoricalPromoStatus>("HistoricalPromoStatuss");
            builder.Entity<PromoStatus>().Collection.Action("ExportXLSX");
            builder.Entity<PromoStatus>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoStatus>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoStatus>("PromoStatuss");
            builder.Entity<HistoricalPromoStatus>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoStatus>("HistoricalPromoStatuss");

            builder.EntitySet<RejectReason>("RejectReasons").HasManyBinding(g => g.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<RejectReason>("DeletedRejectReasons").HasManyBinding(g => g.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<HistoricalRejectReason>("HistoricalRejectReasons");
            builder.Entity<RejectReason>().Collection.Action("ExportXLSX");
            builder.Entity<RejectReason>().Collection.Action("FullImportXLSX");
            builder.Entity<RejectReason>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<RejectReason>("RejectReasons");
            builder.Entity<HistoricalRejectReason>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalRejectReason>("HistoricalRejectReasons");

            builder.EntitySet<Product>("Products").HasManyBinding(e => e.AssortmentMatrices, "AssortmentMatrices");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.AssortmentMatrices, "AssortmentMatrices");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.BaseLines, "BaseLines");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.BaseLines, "BaseLines");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.IncreaseBaseLines, "IncreaseBaseLines");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.IncreaseBaseLines, "IncreaseBaseLines");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.IncrementalPromoes, "IncrementalPromoes");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.IncrementalPromoes, "IncrementalPromoes");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.PreviousDayIncrementals, "PreviousDayIncrementals");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.PreviousDayIncrementals, "PreviousDayIncrementals");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.PriceLists, "PriceLists");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.PriceLists, "PriceLists");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.ProductChangeIncidents, "ProductChangeIncidents");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.ProductChangeIncidents, "ProductChangeIncidents");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.PromoProducts, "PromoProducts");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.PromoProducts, "PromoProducts");
            builder.EntitySet<Product>("Products").HasManyBinding(e => e.RollingVolumes, "RollingVolumes");
            builder.EntitySet<Product>("DeletedProducts").HasManyBinding(e => e.RollingVolumes, "RollingVolumes");
            builder.EntitySet<HistoricalProduct>("HistoricalProducts");
            builder.Entity<Product>().Collection.Action("ExportXLSX");
            builder.Entity<Product>().Collection.Action("FullImportXLSX");
            builder.Entity<Product>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Product>("Products");
            builder.Entity<HistoricalProduct>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalProduct>("HistoricalProducts");
            var getIfAllProductsInSubrangeAction = builder.Entity<Product>().Collection.Action("GetIfAllProductsInSubrange");
            getIfAllProductsInSubrangeAction.Parameter<string>("PromoId");
            getIfAllProductsInSubrangeAction.Parameter<string>("ProductIds");
            getIfAllProductsInSubrangeAction.Parameter<string>("ClientTreeKeyId");
            getIfAllProductsInSubrangeAction.Parameter<string>("DispatchesStart");
            getIfAllProductsInSubrangeAction.Parameter<string>("DispatchesEnd");

            builder.EntitySet<Region>("Regions");
            builder.EntitySet<Region>("DeletedRegions");
            builder.EntitySet<HistoricalRegion>("HistoricalRegions");
            builder.Entity<Region>().Collection.Action("ExportXLSX");
            builder.Entity<Region>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Region>("Regions");
            builder.Entity<HistoricalRegion>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalRegion>("HistoricalRegions");

            builder.EntitySet<CommercialNet>("CommercialNets");
            builder.EntitySet<CommercialNet>("DeletedCommercialNets");
            builder.EntitySet<HistoricalCommercialNet>("HistoricalCommercialNets");
            builder.Entity<CommercialNet>().Collection.Action("ExportXLSX");
            builder.Entity<CommercialNet>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<CommercialNet>("CommercialNets");
            builder.Entity<HistoricalCommercialNet>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCommercialNet>("HistoricalCommercialNets");

            builder.EntitySet<CommercialSubnet>("CommercialSubnets");
            builder.EntitySet<CommercialSubnet>("DeletedCommercialSubnets");
            builder.EntitySet<HistoricalCommercialSubnet>("HistoricalCommercialSubnets");
            builder.EntitySet<CommercialSubnet>("CommercialSubnets").HasRequiredBinding(e => e.CommercialNet, "CommercialNets");
            builder.Entity<CommercialSubnet>().HasRequired(e => e.CommercialNet, (e, te) => e.CommercialNetId == te.Id);
            builder.EntitySet<CommercialSubnet>("DeletedCommercialSubnets").HasRequiredBinding(e => e.CommercialNet, "CommercialNets");
            builder.Entity<CommercialSubnet>().Collection.Action("ExportXLSX");
            builder.Entity<CommercialSubnet>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<CommercialSubnet>("CommercialSubnets");
            builder.Entity<HistoricalCommercialSubnet>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCommercialSubnet>("HistoricalCommercialSubnets");

            builder.EntitySet<Distributor>("Distributors");
            builder.EntitySet<Distributor>("DeletedDistributors");
            builder.EntitySet<HistoricalDistributor>("HistoricalDistributors");
            builder.Entity<Distributor>().Collection.Action("ExportXLSX");
            builder.Entity<Distributor>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Distributor>("Distributors");
            builder.Entity<HistoricalDistributor>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalDistributor>("HistoricalDistributors");

            builder.EntitySet<StoreType>("StoreTypes");
            builder.EntitySet<StoreType>("DeletedStoreTypes");
            builder.EntitySet<HistoricalStoreType>("HistoricalStoreTypes");
            builder.Entity<StoreType>().Collection.Action("ExportXLSX");
            builder.Entity<StoreType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<StoreType>("StoreTypes");
            builder.Entity<HistoricalStoreType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalStoreType>("HistoricalStoreTypes");

            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Client>("DeletedClients");
            builder.EntitySet<HistoricalClient>("HistoricalClients");
            builder.EntitySet<Client>("Clients").HasRequiredBinding(e => e.CommercialSubnet, "CommercialSubnets");
            builder.Entity<Client>().HasRequired(e => e.CommercialSubnet, (e, te) => e.CommercialSubnetId == te.Id);
            builder.EntitySet<Client>("DeletedClients").HasRequiredBinding(e => e.CommercialSubnet, "CommercialSubnets");
            builder.Entity<Client>().Collection.Action("ExportXLSX");
            builder.Entity<Client>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Client>("Clients");
            builder.Entity<HistoricalClient>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalClient>("HistoricalClients");

            builder.EntitySet<Budget>("Budgets").HasManyBinding(g => g.BudgetItems, "BudgetItems");
            builder.EntitySet<Budget>("DeletedBudgets").HasManyBinding(g => g.BudgetItems, "BudgetItems");
            builder.EntitySet<HistoricalBudget>("HistoricalBudgets");
            builder.Entity<Budget>().Collection.Action("ExportXLSX");
            builder.Entity<Budget>().Collection.Action("FullImportXLSX");
            builder.Entity<Budget>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Budget>("Budgets");
            builder.Entity<HistoricalBudget>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBudget>("HistoricalBudgets");

            builder.EntitySet<BudgetItem>("BudgetItems").HasManyBinding(g => g.BudgetSubItems, "BudgetSubItems");
            builder.EntitySet<BudgetItem>("DeletedBudgetItems").HasManyBinding(g => g.BudgetSubItems, "BudgetSubItems");
            builder.EntitySet<BudgetItem>("BudgetItems").HasManyBinding(g => g.NonPromoEquipments, "NonPromoEquipments");
            builder.EntitySet<BudgetItem>("DeletedBudgetItems").HasManyBinding(g => g.NonPromoEquipments, "NonPromoEquipments");
            builder.EntitySet<HistoricalBudgetItem>("HistoricalBudgetItems");
            builder.EntitySet<BudgetItem>("BudgetItems").HasRequiredBinding(e => e.Budget, "Budgets");
            builder.Entity<BudgetItem>().HasRequired(e => e.Budget, (e, te) => e.BudgetId == te.Id);
            builder.EntitySet<BudgetItem>("DeletedBudgetItems").HasRequiredBinding(e => e.Budget, "Budgets");
            builder.Entity<BudgetItem>().Collection.Action("ExportXLSX");
            builder.Entity<BudgetItem>().Collection.Action("FullImportXLSX");
            builder.Entity<BudgetItem>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BudgetItem>("BudgetItems");
            builder.Entity<HistoricalBudgetItem>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBudgetItem>("HistoricalBudgetItems");

            builder.EntitySet<Promo>("Promoes");
            builder.EntitySet<Promo>("DeletedPromoes");
            builder.EntitySet<HistoricalPromo>("HistoricalPromoes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.Technology, "Technologies");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.Technology, "Technologies");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.MarsMechanic, "Mechanics");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.MarsMechanic, "Mechanics");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.MarsMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.MarsMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.PlanInstoreMechanic, "Mechanics");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.PlanInstoreMechanic, "Mechanics");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.PlanInstoreMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.PlanInstoreMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.Color, "Colors");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.Color, "Colors");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.RejectReason, "RejectReasons");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.RejectReason, "RejectReasons");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.Event, "Events");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.Event, "Events");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.ActualInStoreMechanic, "Mechanics");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.ActualInStoreMechanic, "Mechanics");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.ActualInStoreMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.ActualInStoreMechanicType, "MechanicTypes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.PromoTypes, "PromoTypes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.PromoTypes, "PromoTypes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.MasterPromo, "Promoes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.MasterPromo, "DeletedPromoes");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.RollingScenario, "RollingScenarios");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.RollingScenario, "DeletedRollingScenarios");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.SavedScenario, "SavedScenarios");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.SavedScenario, "DeletedSavedScenarios");
            builder.EntitySet<Promo>("Promoes").HasOptionalBinding(e => e.SavedPromo, "SavedPromoes");
            builder.EntitySet<Promo>("DeletedPromoes").HasOptionalBinding(e => e.SavedPromo, "DeletedSavedPromoes");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.Promoes, "Promoes");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.Promoes, "DeletedPromoes");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoProducts, "PromoProducts");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoProducts, "DeletedPromoProducts");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.IncrementalPromoes, "IncrementalPromoes");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.IncrementalPromoes, "IncrementalPromoes");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PreviousDayIncrementals, "PreviousDayIncrementals");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PreviousDayIncrementals, "PreviousDayIncrementals");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoStatusChanges, "PromoStatusChanges");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoSupportPromoes, "PromoSupportPromoes");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoSupportPromoes, "PromoSupportPromoes");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoProductTrees, "PromoProductTrees");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoProductTrees, "PromoProductTrees");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoUpliftFailIncidents, "PromoUpliftFailIncidents");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoUpliftFailIncidents, "PromoUpliftFailIncidents");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoOnApprovalIncidents, "PromoOnApprovalIncidents");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoOnApprovalIncidents, "PromoOnApprovalIncidents");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoOnRejectIncidents, "PromoOnRejectIncidents");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoOnRejectIncidents, "PromoOnRejectIncidents");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoCancelledIncidents, "PromoCancelledIncidents");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoCancelledIncidents, "PromoCancelledIncidents");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.PromoApprovedIncidents, "PromoApprovedIncidents");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.PromoApprovedIncidents, "PromoApprovedIncidents");
            builder.EntitySet<Promo>("Promoes").HasManyBinding(e => e.CurrentDayIncrementals, "CurrentDayIncrementals");
            builder.EntitySet<Promo>("DeletedPromoes").HasManyBinding(e => e.CurrentDayIncrementals, "CurrentDayIncrementals");
            builder.Entity<Promo>().Collection.Action("ExportXLSX");
            builder.Entity<Promo>().Collection.Action("FullImportXLSX");
            builder.Entity<Promo>().Collection.Action("DeclinePromo");
            builder.Entity<Promo>().Collection.Action("GetUserDashboardsCount");
            builder.Entity<Promo>().Collection.Action("GetLiveMetricsDashboard");
            builder.Entity<Promo>().Collection.Action("GetApprovalHistory");
            builder.Entity<Promo>().Collection.Action("CalculateMarketingTI");
            builder.Entity<Promo>().Collection.Action("ChangeStatus");
            builder.Entity<Promo>().Collection.Action("ExportPromoROIReportXLSX");
            builder.Entity<Promo>().Collection.Action("ExportPromoPriceIncreaseROIReportXLSX");
            builder.Entity<Promo>().Collection.Action("RecalculatePromo");
            builder.Entity<Promo>().Collection.Action("ResetPromo");
            builder.Entity<Promo>().Collection.Action("ChangeResponsible");
            builder.Entity<Promo>().Collection.Action("MassApprove");
            builder.Entity<Promo>().Collection.Action("SendForApproval");
            builder.Entity<Promo>().Collection.Action("CheckIfLogHasErrors");
            builder.Entity<Promo>().Collection.Action("CheckPromoCreator");
            builder.Entity<Promo>().Collection.Action("GetProducts").CollectionParameter<string>("InOutProductIds");
            builder.Entity<Promo>().Collection.Action("ReadPromoCalculatingLog");
            builder.Entity<Promo>().Collection.Action("GetHandlerIdForBlockedPromo");
            builder.Entity<Promo>().Collection.Action("GetRSPeriod");
            builder.Entity<Promo>().Collection.Action("PromoRSDelete");
            builder.Entity<Promo>().Collection.Action("InvoiceFilter");
            builder.Entity<Promo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Promo>("Promoes");
            builder.Entity<HistoricalPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromo>("HistoricalPromoes");

            ActionConfiguration schedExp = builder.Entity<PromoView>().Collection.Action("ExportSchedule");
            schedExp.CollectionParameter<int>("clients");
            schedExp.CollectionParameter<string>("competitors");
            schedExp.CollectionParameter<string>("types");
            schedExp.Parameter<int?>("year");

            ActionConfiguration schedExpRS = builder.Entity<PromoRSView>().Collection.Action("ExportSchedule");
            schedExpRS.CollectionParameter<int>("clients");
            schedExpRS.CollectionParameter<string>("competitors");
            schedExpRS.CollectionParameter<string>("types");
            schedExpRS.Parameter<int?>("year");

            builder.EntitySet<Sale>("Sales");
            builder.EntitySet<Sale>("DeletedSales");
            builder.EntitySet<HistoricalSale>("HistoricalSales");
            builder.EntitySet<Sale>("Sales").HasOptionalBinding(e => e.Promo, "Promoes");
            builder.EntitySet<Sale>("DeletedSales").HasOptionalBinding(e => e.Promo, "Promoes");
            builder.EntitySet<Sale>("Sales").HasOptionalBinding(e => e.Budget, "Budgets");
            builder.EntitySet<Sale>("DeletedSales").HasOptionalBinding(e => e.Budget, "Budgets");
            builder.EntitySet<Sale>("Sales").HasOptionalBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<Sale>("DeletedSales").HasOptionalBinding(e => e.BudgetItem, "BudgetItems");
            builder.Entity<Sale>().Collection.Action("ExportXLSX");
            builder.Entity<Sale>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Sale>("Sales");
            builder.Entity<HistoricalSale>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalSale>("HistoricalSales");

            builder.EntitySet<PromoSales>("PromoSaleses");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses");
            builder.EntitySet<HistoricalPromoSales>("HistoricalPromoSaleses");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.Client, "Clients");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.Mechanic, "Mechanics");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.EntitySet<PromoSales>("PromoSaleses").HasOptionalBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.Client, "Clients");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.Mechanic, "Mechanics");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.EntitySet<PromoSales>("DeletedPromoSaleses").HasOptionalBinding(e => e.BudgetItem, "BudgetItems");
            builder.Entity<PromoSales>().Collection.Action("ExportXLSX");
            builder.Entity<PromoSales>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoSales>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoSales>("PromoSaleses");
            builder.Entity<HistoricalPromoSales>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoSales>("HistoricalPromoSaleses");

            builder.EntitySet<Demand>("Demands");
            builder.EntitySet<Demand>("DeletedDemands");
            builder.EntitySet<HistoricalPromo>("HistoricalDemands");
            builder.EntitySet<Demand>("Demands").HasOptionalBinding(e => e.Client, "Clients");
            builder.EntitySet<Demand>("Demands").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<Demand>("Demands").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<Demand>("DeletedDemands").HasOptionalBinding(e => e.Client, "Clients");
            builder.EntitySet<Demand>("DeletedDemands").HasOptionalBinding(e => e.Brand, "Brands");
            builder.EntitySet<Demand>("DeletedDemands").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<Demand>().Collection.Action("ExportXLSX");
            builder.Entity<Demand>().Collection.Action("FullImportXLSX");
            builder.Entity<Demand>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Demand>("Demands");

            builder.EntitySet<Color>("Colors");
            builder.EntitySet<Color>("DeletedColors");
            builder.EntitySet<HistoricalColor>("HistoricalColors");
            builder.EntitySet<Color>("Colors").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<Color>("DeletedColors").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<Color>().Collection.Action("GetSuitable");
            builder.Entity<Color>().Collection.Action("FullImportXLSX");
            builder.Entity<Color>().Collection.Action("ExportXLSX");
            builder.Entity<Color>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Color>("Colors");
            builder.Entity<HistoricalColor>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalColor>("HistoricalColors");

            builder.EntitySet<RollingVolume>("RollingVolumes");
            builder.EntitySet<RollingVolume>("RollingVolumes").HasOptionalBinding(e => e.Product, "Products");
            builder.Entity<RollingVolume>().Collection.Action("FullImportXLSX");
            builder.Entity<RollingVolume>().Collection.Action("ExportXLSX");
            builder.Entity<RollingVolume>().Collection.Action("IsRollingVolumeDay");
            builder.Entity<RollingVolume>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<RollingVolume>("RollingVolumes");

            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(e => e.AssortmentMatrices, "AssortmentMatrices");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(e => e.AssortmentMatrices, "AssortmentMatrices"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(e => e.AssortmentMatrices, "AssortmentMatrices");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(e => e.BudgetSubItemClientTrees, "BudgetSubItemClientTrees");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(e => e.BudgetSubItemClientTrees, "BudgetSubItemClientTrees"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(e => e.BudgetSubItemClientTrees, "BudgetSubItemClientTrees");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.ClientTreeBrandTeches, "ClientTreeBrandTeches");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.ClientTreeBrandTeches, "ClientTreeBrandTeches"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.ClientTreeBrandTeches, "ClientTreeBrandTeches");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.EventClientTrees, "EventClientTrees");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.EventClientTrees, "EventClientTrees"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.EventClientTrees, "EventClientTrees");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.MechanicTypes, "MechanicTypes");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.MechanicTypes, "MechanicTypes"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.MechanicTypes, "MechanicTypes");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.NoneNegoes, "NoneNegoes"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.NonPromoSupports, "NonPromoSupports");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.NonPromoSupports, "NonPromoSupports"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.NonPromoSupports, "NonPromoSupports");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.Plus, "Plus");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.Plus, "Plus"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.Plus, "Plus");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.PriceLists, "PriceLists");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.PriceLists, "PriceLists"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.PriceLists, "PriceLists");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.PromoSupports, "PromoSupports");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.PromoSupports, "PromoSupports"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.PromoSupports, "PromoSupports");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.RATIShoppers, "RATIShoppers");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.RATIShoppers, "RATIShoppers"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.RATIShoppers, "RATIShoppers");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.TradeInvestments, "TradeInvestments");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.TradeInvestments, "TradeInvestments"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.TradeInvestments, "TradeInvestments");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.PlanPostPromoEffects, "PlanPostPromoEffects");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.PlanPostPromoEffects, "PlanPostPromoEffects"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.PlanPostPromoEffects, "PlanPostPromoEffects");
            builder.EntitySet<ClientTree>("ClientTrees").HasManyBinding(g => g.SavedPromos, "SavedPromoes");
            builder.EntitySet<ClientTree>("BaseClients").HasManyBinding(g => g.SavedPromos, "SavedPromoes"); // Для получение только базовых клиентов из иерархии
            builder.EntitySet<ClientTree>("BaseClientViews").HasManyBinding(g => g.SavedPromos, "SavedPromoes");
            builder.Entity<ClientTree>().Collection.Action("Delete");
            builder.Entity<ClientTree>().Collection.Action("Move");
            ActionConfiguration updateClientNodeAction = builder.Entity<ClientTree>().Collection.Action("UpdateNode");
            updateClientNodeAction.ReturnsFromEntitySet<ClientTree>("ClientTrees"); //какого типа параметр принимает экшн.
            builder.Entity<ClientTree>().Collection.Action("GetClientTreeByObjectId");
            builder.Entity<ClientTree>().Collection.Action("CanCreateBaseClient");
            builder.Entity<ClientTree>().Collection.Action("UploadLogoFile");
            builder.Entity<ClientTree>().Collection.Action("DownloadLogoFile");
            builder.Entity<ClientTree>().Collection.Action("DeleteLogo");
            builder.Entity<ClientTree>().Collection.Action("GetUploadingClients");
            builder.Entity<ClientTree>().Collection.Action("SaveScenario");
            builder.Entity<ClientTree>().Collection.Action("CopyYearScenario");
            builder.Entity<BaseClientTreeView>().Collection.Action("ExportXLSX");
            builder.Entity<ClientTree>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientTree>("ClientTrees");

            builder.EntitySet<ProductTree>("ProductTrees").HasManyBinding(g => g.NoneNegoes, "NoneNegoes");
            builder.EntitySet<ProductTree>("ProductTrees").HasRequiredBinding(e => e.Technology, "Technologies");
            builder.Entity<ProductTree>().Collection.Action("Delete");
            builder.Entity<ProductTree>().Collection.Action("Move");
            builder.Entity<ProductTree>().Collection.Action("UploadLogoFile");
            builder.Entity<ProductTree>().Collection.Action("DownloadLogoFile");
            builder.Entity<ProductTree>().Collection.Action("DeleteLogo");
            ActionConfiguration updateProductNodeAction = builder.Entity<ProductTree>().Collection.Action("UpdateNode");
            updateProductNodeAction.ReturnsFromEntitySet<ProductTree>("ProductTrees"); //какого типа параметр принимает экшн.
            builder.Entity<ProductTree>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ProductTree>("ProductTrees");

            builder.EntitySet<NodeType>("NodeTypes");
            builder.EntitySet<NodeType>("DeletedNodeTypes");
            builder.EntitySet<HistoricalNodeType>("HistoricalNodeTypes");
            builder.Entity<NodeType>().Collection.Action("FullImportXLSX");
            builder.Entity<NodeType>().Collection.Action("ExportXLSX");
            builder.Entity<NodeType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<NodeType>("NodeTypes");
            builder.Entity<HistoricalNodeType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalNodeType>("HistoricalNodeTypes");

            builder.EntitySet<PromoStatusChange>("PromoStatusChanges");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.RejectReason, "RejectReasons");
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("UserName"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("RoleName"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("StatusColor"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("StatusName"));
            builder.Entity<PromoStatusChange>().Collection.Action("PromoStatusChangesByPromo");
            builder.Entity<PromoStatusChange>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoStatusChange>("PromoStatusChanges");

            builder.EntitySet<PromoDemand>("PromoDemands");
            builder.EntitySet<PromoDemand>("DeletedPromoDemands");
            builder.EntitySet<HistoricalPromoDemand>("HistoricalPromoDemands");
            builder.EntitySet<PromoDemand>("PromoDemands").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<PromoDemand>().HasRequired(e => e.BrandTech, (e, te) => e.BrandTechId == te.Id);
            builder.EntitySet<PromoDemand>("PromoDemands").HasRequiredBinding(e => e.Mechanic, "Mechanics");
            builder.Entity<PromoDemand>().HasRequired(e => e.Mechanic, (e, te) => e.MechanicId == te.Id);
            builder.EntitySet<PromoDemand>("PromoDemands").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.EntitySet<PromoDemand>("DeletedPromoDemands").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<PromoDemand>("DeletedPromoDemands").HasRequiredBinding(e => e.Mechanic, "Mechanics");
            builder.EntitySet<PromoDemand>("DeletedPromoDemands").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.Entity<PromoDemand>().Collection.Action("ExportXLSX");
            builder.Entity<PromoDemand>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoDemand>("PromoDemands");
            builder.Entity<HistoricalPromoDemand>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoDemand>("HistoricalPromoDemands");

            builder.EntitySet<BaseClientTreeView>("BaseClientTreeViews");
            builder.Entity<BaseClientTreeView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BaseClientTreeView>("BaseClientTreeViews");
            builder.EntitySet<ClientTreeSharesView>("ClientTreeSharesViews");
            builder.Entity<ClientTreeSharesView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientTreeSharesView>("ClientTreeSharesViews");


            builder.EntitySet<ClientTreeBrandTech>("ClientTreeBrandTeches");
            builder.EntitySet<ClientTreeBrandTech>("DeletedClientTreeBrandTeches");
            builder.EntitySet<HistoricalClientTreeBrandTech>("HistoricalClientTreeBrandTeches");
            builder.Entity<ClientTreeBrandTech>().Collection.Action("ExportXLSX");
            builder.Entity<ClientTreeBrandTech>().Collection.Action("FullImportXLSX");
            builder.EntitySet<ClientTreeBrandTech>("ClientTreeBrandTeches").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ClientTreeBrandTech>("DeletedClientTreeBrandTeches").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.Entity<ClientTreeBrandTech>().HasRequired(e => e.ClientTree, (e, te) => e.ClientTreeId == te.Id);
            builder.EntitySet<ClientTreeBrandTech>("ClientTreeBrandTeches").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<ClientTreeBrandTech>("DeletedClientTreeBrandTeches").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<ClientTreeBrandTech>().HasRequired(e => e.BrandTech, (e, te) => e.BrandTechId == te.Id);
            builder.Entity<ClientTreeBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientTreeBrandTech>("ClientTreeBrandTeches");
            builder.Entity<HistoricalClientTreeBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalClientTreeBrandTech>("HistoricalClientTreeBrandTeches");

            builder.EntitySet<NoneNego>("NoneNegoes");
            builder.EntitySet<NoneNego>("DeletedNoneNegoes");
            builder.EntitySet<HistoricalNoneNego>("HistoricalNoneNegoes");
            builder.EntitySet<NoneNego>("NoneNegoes").HasOptionalBinding(e => e.Mechanic, "Mechanics");
            builder.EntitySet<NoneNego>("NoneNegoes").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.EntitySet<NoneNego>("DeletedNoneNegoes").HasOptionalBinding(e => e.Mechanic, "Mechanics");
            builder.EntitySet<NoneNego>("DeletedNoneNegoes").HasOptionalBinding(e => e.MechanicType, "MechanicTypes");
            builder.EntitySet<NoneNego>("NoneNegoes").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<NoneNego>("NoneNegoes").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.EntitySet<NoneNego>("DeletedNoneNegoes").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<NoneNego>("DeletedNoneNegoes").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.Entity<NoneNego>().Collection.Action("ExportXLSX");
            builder.Entity<NoneNego>().Collection.Action("IsValidPeriod");
            builder.Entity<NoneNego>().Collection.Action("FullImportXLSX");
            builder.Entity<NoneNego>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<HistoricalNoneNego>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalNoneNego>("HistoricalNoneNegoes");
            builder.Entity<NoneNego>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<NoneNego>("NoneNegoes");

            builder.EntitySet<PLUDictionary>("PLUDictionaries");
            builder.Entity<PLUDictionary>().Collection.Action("ExportXLSX");
            builder.Entity<PLUDictionary>().Collection.Action("FullImportXLSX");
            builder.Entity<PLUDictionary>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PLUDictionary>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PLUDictionary>("PLUDictionaries");


            builder.EntitySet<PromoProduct2Plu>("PromoProduct2Plus");

            builder.EntitySet<ExportQuery>("ExportQueries");

            builder.EntitySet<PromoProduct>("PromoProducts").HasManyBinding(g => g.PromoProductsCorrections, "PromoProductsCorrections");
            builder.EntitySet<PromoProduct>("DeletedPromoProducts").HasManyBinding(g => g.PromoProductsCorrections, "PromoProductsCorrections");
            builder.EntitySet<HistoricalPromoProduct>("HistoricalPromoProducts");
            builder.EntitySet<PromoProduct>("PromoProducts").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoProduct>("PromoProducts").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<PromoProduct>("DeletedPromoProducts").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoProduct>("DeletedPromoProducts").HasRequiredBinding(e => e.Product, "Products");
            //builder.EntitySet<PromoProduct>("PromoProducts").EntityType.Ignore(e => e.ActualProductBaselineLSV);
            //builder.StructuralTypes.First(e => e.ClrType == typeof(PromoProduct)).AddProperty(typeof(PromoProduct).GetProperty("ActualProductBaselineLSV"));
            builder.Entity<PromoProduct>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.Entity<PromoProduct>().HasRequired(n => n.Product, (n, p) => n.ProductId == p.Id);
            builder.Entity<PromoProduct>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProduct>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProduct>().Collection.Action("FullImportPluXLSX");
            builder.Entity<PromoProduct>().Collection.Action("DownloadTemplateXLSXTLC");
            builder.Entity<PromoProduct>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProduct>().Collection.Action("DownloadTemplatePluXLSX");
            builder.Entity<PromoProduct>().Collection.Action("SupportAdminExportXLSX");
            builder.Entity<PromoProduct>().Collection.Action("GetPromoProductByPromoAndProduct");
            builder.Entity<PromoProduct>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProduct>("PromoProducts");
            builder.Entity<HistoricalPromoProduct>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoProduct>("HistoricalPromoProducts");

            //builder.Entity<PromoProduct>().HasOptional(x => x.Plu, (n, p) => n.Id == p.Id);


            builder.EntitySet<PromoProductsView>("PromoProductsViews");
            builder.Entity<PromoProductsView>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProductsView>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProductsView>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProductsView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProductsView>("PromoProductsViews");

            builder.EntitySet<BaseLine>("BaseLines");
            builder.EntitySet<BaseLine>("DeletedBaseLines");
            builder.EntitySet<BaseLine>("BaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<BaseLine>("DeletedBaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<HistoricalBaseLine>("HistoricalBaseLines");
            builder.Entity<BaseLine>().Collection.Action("ExportXLSX");
            builder.Entity<BaseLine>().Collection.Action("FullImportXLSX");
            builder.Entity<BaseLine>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BaseLine>("BaseLines");
            builder.Entity<HistoricalBaseLine>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBaseLine>("HistoricalBaseLines");

            builder.EntitySet<IncreaseBaseLine>("IncreaseBaseLines");
            builder.EntitySet<IncreaseBaseLine>("DeletedIncreaseBaseLines");
            builder.EntitySet<IncreaseBaseLine>("IncreaseBaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<IncreaseBaseLine>("DeletedIncreaseBaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<HistoricalIncreaseBaseLine>("HistoricalIncreaseBaseLines");
            builder.Entity<IncreaseBaseLine>().Collection.Action("ExportXLSX");
            builder.Entity<IncreaseBaseLine>().Collection.Action("FullImportXLSX");
            builder.Entity<IncreaseBaseLine>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<IncreaseBaseLine>("IncreaseBaseLines");
            builder.Entity<HistoricalIncreaseBaseLine>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalIncreaseBaseLine>("HistoricalIncreaseBaseLines");

            builder.EntitySet<RetailType>("RetailTypes");
            builder.EntitySet<RetailType>("DeletedRetailTypes");
            builder.EntitySet<HistoricalRetailType>("HistoricalRetailTypes");
            builder.Entity<RetailType>().Collection.Action("ExportXLSX");
            builder.Entity<RetailType>().Collection.Action("FullImportXLSX");
            builder.Entity<RetailType>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<RetailType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<RetailType>("RetailTypes");
            builder.Entity<HistoricalRetailType>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalRetailType>("HistoricalRetailTypes");

            builder.EntitySet<ProductChangeIncident>("ProductChangeIncidents").HasRequiredBinding(e => e.Product, "Products");
            builder.Entity<ProductChangeIncident>().HasRequired(e => e.Product, (e, te) => e.ProductId == te.Id);

            builder.EntitySet<BudgetSubItem>("BudgetSubItems").HasManyBinding(g => g.BudgetSubItemClientTrees, "BudgetSubItemClientTrees");
            builder.EntitySet<BudgetSubItem>("DeletedBudgetSubItems").HasManyBinding(g => g.BudgetSubItemClientTrees, "BudgetSubItemClientTrees");
            builder.EntitySet<BudgetSubItem>("BudgetSubItems").HasManyBinding(g => g.PromoSupports, "PromoSupports");
            builder.EntitySet<BudgetSubItem>("DeletedBudgetSubItems").HasManyBinding(g => g.PromoSupports, "PromoSupports");
            builder.EntitySet<HistoricalBudgetSubItem>("HistoricalBudgetSubItems");
            builder.EntitySet<BudgetSubItem>("BudgetSubItems").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<BudgetSubItem>("DeletedBudgetSubItems").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.Entity<BudgetSubItem>().HasRequired(e => e.BudgetItem, (e, bi) => e.BudgetItemId == bi.Id);
            builder.Entity<BudgetSubItem>().Collection.Action("ExportXLSX");
            builder.Entity<BudgetSubItem>().Collection.Action("FullImportXLSX");
            builder.Entity<BudgetSubItem>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BudgetSubItem>("BudgetSubItems");
            builder.Entity<HistoricalBudgetSubItem>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBudgetSubItem>("HistoricalBudgetSubItems");

            builder.EntitySet<PreviousDayIncremental>("PreviousDayIncrementals");
            builder.Entity<PreviousDayIncremental>().Collection.Action("ExportXLSX");
            builder.EntitySet<PreviousDayIncremental>("PreviousDayIncrementals").HasOptionalBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PreviousDayIncremental>("PreviousDayIncrementals").HasOptionalBinding(e => e.Product, "Products");
            builder.Entity<PreviousDayIncremental>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PreviousDayIncremental>("PreviousDayIncrementals");

            builder.EntitySet<PromoProductsCorrection>("PromoProductsCorrections");
            builder.EntitySet<PromoProductsCorrection>("DeletedPromoProductsCorrections");
            builder.EntitySet<PromoProductsCorrection>("PromoProductsCorrections").HasOptionalBinding(e => e.PromoProduct, "PromoProducts");
            builder.EntitySet<PromoProductsCorrection>("DeletedPromoProductsCorrections").HasOptionalBinding(e => e.PromoProduct, "PromoProducts");
            builder.Entity<PromoProductsCorrection>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProductsCorrection>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProductsCorrection>().Collection.Action("ExportCorrectionXLSX");
            builder.Entity<PromoProductsCorrection>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProductsCorrection>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProductsCorrection>("PromoProductsCorrections");
            builder.EntitySet<HistoricalPromoProductsCorrection>("HistoricalPromoProductsCorrections");
            builder.Entity<HistoricalPromoProductsCorrection>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoProductsCorrection>("HistoricalPromoProductsCorrections");

            builder.EntitySet<PromoSupport>("PromoSupports");
            builder.EntitySet<PromoSupport>("DeletedPromoSupports");
            builder.EntitySet<HistoricalPromoSupport>("HistoricalPromoSupports");
            builder.EntitySet<PromoSupport>("PromoSupports").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PromoSupport>("DeletedPromoSupports").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PromoSupport>("PromoSupports").HasOptionalBinding(e => e.BudgetSubItem, "BudgetSubItems");
            builder.EntitySet<PromoSupport>("DeletedPromoSupports").HasOptionalBinding(e => e.BudgetSubItem, "BudgetSubItems");
            builder.Entity<PromoSupport>().Collection.Action("ExportXLSX");
            builder.Entity<PromoSupport>().Collection.Action("GetPromoSupportGroup");
            builder.Entity<PromoSupport>().Collection.Action("UploadFile");
            builder.Entity<PromoSupport>().Collection.Action("DownloadFile");
            builder.Entity<PromoSupport>().Collection.Action("GetUserTimestamp");
            builder.EntitySet<PromoSupport>("PromoSupports").HasManyBinding<PromoSupportPromo>(e => e.PromoSupportPromo, "PromoSupportPromoes");
            builder.EntitySet<PromoSupport>("DeletedPromoSupports").HasManyBinding<PromoSupportPromo>(e => e.PromoSupportPromo, "PromoSupportPromoes");
            builder.Entity<PromoSupport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoSupport>("PromoSupports");
            builder.Entity<HistoricalPromoSupport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPromoSupport>("HistoricalPromoSupports");

            builder.EntitySet<NonPromoSupport>("NonPromoSupports").HasManyBinding(g => g.NonPromoSupportBrandTeches, "NonPromoSupportBrandTeches");
            builder.EntitySet<NonPromoSupport>("DeletedNonPromoSupports").HasManyBinding(g => g.NonPromoSupportBrandTeches, "NonPromoSupportBrandTeches");
            builder.EntitySet<HistoricalNonPromoSupport>("HistoricalNonPromoSupports");
            builder.EntitySet<NonPromoSupport>("NonPromoSupports").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<NonPromoSupport>("DeletedNonPromoSupports").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<NonPromoSupport>("NonPromoSupports").HasOptionalBinding(e => e.NonPromoEquipment, "NonPromoEquipments");
            builder.EntitySet<NonPromoSupport>("DeletedNonPromoSupports").HasOptionalBinding(e => e.NonPromoEquipment, "NonPromoEquipments");
            builder.Entity<NonPromoSupport>().Collection.Action("ExportXLSX");
            builder.Entity<NonPromoSupport>().Collection.Action("FullImportXLSX");
            builder.Entity<NonPromoSupport>().Collection.Action("GetNonPromoSupportGroup");
            builder.Entity<NonPromoSupport>().Collection.Action("UploadFile");
            builder.Entity<NonPromoSupport>().Collection.Action("DownloadFile");
            builder.Entity<NonPromoSupport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<NonPromoSupport>("NonPromoSupports");
            builder.Entity<HistoricalNonPromoSupport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalNonPromoSupport>("HistoricalNonPromoSupports");

            builder.EntitySet<NonPromoSupportBrandTech>("NonPromoSupportBrandTeches");
            builder.EntitySet<NonPromoSupportBrandTech>("DeletedNonPromoSupportBrandTeches");
            builder.EntitySet<NonPromoSupportBrandTech>("NonPromoSupportBrandTeches").HasRequiredBinding(e => e.NonPromoSupport, "NonPromoSupports");
            builder.EntitySet<NonPromoSupportBrandTech>("NonPromoSupportBrandTeches").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<NonPromoSupportBrandTech>("DeletedNonPromoSupportBrandTeches").HasRequiredBinding(e => e.NonPromoSupport, "NonPromoSupports");
            builder.EntitySet<NonPromoSupportBrandTech>("DeletedNonPromoSupportBrandTeches").HasRequiredBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<NonPromoSupportBrandTech>().HasRequired(n => n.NonPromoSupport, (n, nps) => n.NonPromoSupportId == nps.Id);
            builder.Entity<NonPromoSupportBrandTech>().HasRequired(n => n.BrandTech, (n, bt) => n.NonPromoSupportId == bt.Id);
            builder.Entity<NonPromoSupportBrandTech>().Collection.Action("ModifyNonPromoSupportBrandTechList");
            builder.Entity<NonPromoSupportBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<NonPromoSupportBrandTech>("NonPromoSupportBrandTeches");

            builder.EntitySet<PostPromoEffect>("PostPromoEffects");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects");
            builder.EntitySet<HistoricalPostPromoEffect>("HistoricalPostPromoEffects");
            builder.EntitySet<PostPromoEffect>("PostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PostPromoEffect>("PostPromoEffects").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.Entity<PostPromoEffect>().Collection.Action("ExportXLSX");
            builder.Entity<PostPromoEffect>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PostPromoEffect>("PostPromoEffects");
            builder.Entity<HistoricalPostPromoEffect>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPostPromoEffect>("HistoricalPostPromoEffects");

            builder.EntitySet<PromoSupportPromo>("PromoSupportPromoes");
            builder.EntitySet<PromoSupportPromo>("DeletedPromoSupportPromoes");
            builder.EntitySet<HistoricalPromoSupportPromo>("HistoricalPromoSupportPromoes");
            builder.EntitySet<PromoSupportPromo>("PromoSupportPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoSupportPromo>("PromoSupportPromoes").HasRequiredBinding(e => e.PromoSupport, "PromoSupports");
            builder.EntitySet<PromoSupportPromo>("DeletedPromoSupportPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoSupportPromo>("DeletedPromoSupportPromoes").HasRequiredBinding(e => e.PromoSupport, "PromoSupports");
            builder.Entity<PromoSupportPromo>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.Entity<PromoSupportPromo>().HasRequired(n => n.PromoSupport, (n, ps) => n.PromoSupportId == ps.Id);
            builder.Entity<PromoSupportPromo>().Collection.Action("GetLinkedSubItems");
            builder.Entity<PromoSupportPromo>().Collection.Action("ManageSubItems");
            builder.Entity<PromoSupportPromo>().Collection.Action("GetValuesForItems");
            builder.Entity<PromoSupportPromo>().Collection.Action("PromoSuportPromoPost");
            builder.Entity<PromoSupportPromo>().Collection.Action("ExportXLSX");
            builder.Entity<PromoSupportPromo>().Collection.Action("ChangeListPSP");
            builder.Entity<PromoSupportPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoSupportPromo>("PromoSupportPromoes");
            builder.Entity<PromoSupportPromo>().Collection.Action("PromoSupportPromoDelete");

            builder.EntitySet<COGS>("COGSs");
            builder.EntitySet<COGS>("DeletedCOGSs");
            builder.EntitySet<HistoricalCOGS>("HistoricalCOGSs");
            builder.Entity<COGS>().Collection.Action("ExportXLSX");
            builder.EntitySet<COGS>("COGSs").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<COGS>("DeletedCOGSs").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<COGS>("COGSs").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<COGS>("DeletedCOGSs").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<COGS>().Collection.Action("FullImportXLSX");
            builder.Entity<COGS>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<COGS>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<COGS>("COGSs");
            builder.Entity<HistoricalCOGS>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCOGS>("HistoricalCOGSs");

            builder.EntitySet<PlanPostPromoEffect>("PlanPostPromoEffects");
            builder.EntitySet<PlanPostPromoEffect>("DeletedPlanPostPromoEffects");
            builder.EntitySet<HistoricalPlanPostPromoEffect>("HistoricalPlanPostPromoEffects");
            builder.Entity<PlanPostPromoEffect>().Collection.Action("ExportXLSX");
            builder.EntitySet<PlanPostPromoEffect>("PlanPostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PlanPostPromoEffect>("DeletedPlanPostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PlanPostPromoEffect>("PlanPostPromoEffects").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<PlanPostPromoEffect>("DeletedPlanPostPromoEffects").HasOptionalBinding(e => e.BrandTech, "BrandTeches");

            builder.EntitySet<PlanPostPromoEffect>("PlanPostPromoEffects").HasRequiredBinding(e => e.DurationRange, "DurationRanges");
            builder.EntitySet<PlanPostPromoEffect>("DeletedPlanPostPromoEffects").HasRequiredBinding(e => e.DurationRange, "DurationRanges");
            builder.EntitySet<PlanPostPromoEffect>("PlanPostPromoEffects").HasRequiredBinding(e => e.DiscountRange, "DiscountRanges");
            builder.EntitySet<PlanPostPromoEffect>("DeletedPlanPostPromoEffects").HasRequiredBinding(e => e.DiscountRange, "DiscountRanges");

            builder.Entity<PlanPostPromoEffect>().Collection.Action("DownloadTemplateXLSX");

            builder.Entity<PlanPostPromoEffect>().Collection.Action("GetBrandTechSizes");
            builder.Entity<PlanPostPromoEffect>().Collection.Action("FullImportXLSX");
            builder.Entity<PlanPostPromoEffect>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PlanPostPromoEffect>("PlanPostPromoEffects");
            builder.Entity<HistoricalPlanPostPromoEffect>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPlanPostPromoEffect>("HistoricalPlanPostPromoEffects");
            builder.EntitySet<DurationRange>("DurationRanges").HasManyBinding(e => e.PlanPostPromoEffects, "PlanPostPromoEffects");
            builder.EntitySet<DiscountRange>("DiscountRanges").HasManyBinding(e => e.PlanPostPromoEffects, "PlanPostPromoEffects");

            builder.EntitySet<PlanCOGSTn>("PlanCOGSTns");
            builder.EntitySet<PlanCOGSTn>("DeletedPlanCOGSTns");
            builder.EntitySet<HistoricalPlanCOGSTn>("HistoricalPlanCOGSTns");
            builder.Entity<PlanCOGSTn>().Collection.Action("ExportXLSX");
            builder.EntitySet<PlanCOGSTn>("PlanCOGSTns").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PlanCOGSTn>("DeletedPlanCOGSTns").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PlanCOGSTn>("PlanCOGSTns").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<PlanCOGSTn>("DeletedPlanCOGSTns").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<PlanCOGSTn>().Collection.Action("FullImportXLSX");
            builder.Entity<PlanCOGSTn>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PlanCOGSTn>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PlanCOGSTn>("PlanCOGSTns");
            builder.Entity<HistoricalPlanCOGSTn>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPlanCOGSTn>("HistoricalPlanCOGSTns");

            builder.EntitySet<TradeInvestment>("TradeInvestments");
            builder.EntitySet<TradeInvestment>("DeletedTradeInvestments");
            builder.EntitySet<HistoricalTradeInvestment>("HistoricalTradeInvestments");
            builder.Entity<TradeInvestment>().Collection.Action("ExportXLSX");
            builder.EntitySet<TradeInvestment>("TradeInvestments").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<TradeInvestment>("DeletedTradeInvestments").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<TradeInvestment>("TradeInvestments").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<TradeInvestment>("DeletedTradeInvestments").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<TradeInvestment>().Collection.Action("FullImportXLSX");
            builder.Entity<TradeInvestment>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<TradeInvestment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<TradeInvestment>("TradeInvestments");
            builder.Entity<HistoricalTradeInvestment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalTradeInvestment>("HistoricalTradeInvestments");

            builder.EntitySet<ActualCOGS>("ActualCOGSs");
            builder.EntitySet<ActualCOGS>("DeletedActualCOGSs");
            builder.EntitySet<HistoricalActualCOGS>("HistoricalActualCOGSs");
            builder.Entity<ActualCOGS>().Collection.Action("ExportXLSX");
            builder.EntitySet<ActualCOGS>("ActualCOGSs").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualCOGS>("DeletedActualCOGSs").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualCOGS>("ActualCOGSs").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<ActualCOGS>("DeletedActualCOGSs").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<ActualCOGS>().Collection.Action("FullImportXLSX");
            builder.Entity<ActualCOGS>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<ActualCOGS>().Collection.Action("IsCOGSRecalculatePreviousYearButtonAvailable");
            builder.Entity<ActualCOGS>().Collection.Action("PreviousYearPromoList");
            builder.Entity<ActualCOGS>().Collection.Action("CreateActualCOGSChangeIncidents");
            builder.Entity<ActualCOGS>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ActualCOGS>("ActualCOGSs");
            builder.Entity<HistoricalActualCOGS>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalActualCOGS>("HistoricalActualCOGSs");

            builder.EntitySet<ActualCOGSTn>("ActualCOGSTns");
            builder.EntitySet<ActualCOGSTn>("DeletedActualCOGSTns");
            builder.EntitySet<HistoricalActualCOGSTn>("HistoricalActualCOGSTns");
            builder.Entity<ActualCOGSTn>().Collection.Action("ExportXLSX");
            builder.EntitySet<ActualCOGSTn>("ActualCOGSTns").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualCOGSTn>("DeletedActualCOGSTns").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualCOGSTn>("ActualCOGSTns").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<ActualCOGSTn>("DeletedActualCOGSTns").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<ActualCOGSTn>().Collection.Action("FullImportXLSX");
            builder.Entity<ActualCOGSTn>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<ActualCOGSTn>().Collection.Action("IsCOGSTnRecalculatePreviousYearButtonAvailable");
            builder.Entity<ActualCOGSTn>().Collection.Action("PreviousYearPromoList");
            builder.Entity<ActualCOGSTn>().Collection.Action("CreateActualCOGSTnChangeIncidents");
            builder.Entity<ActualCOGSTn>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ActualCOGSTn>("ActualCOGSTns");
            builder.Entity<HistoricalActualCOGSTn>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalActualCOGSTn>("HistoricalActualCOGSTns");

            builder.EntitySet<RATIShopper>("RATIShoppers");
            builder.EntitySet<RATIShopper>("DeletedRATIShoppers");
            builder.EntitySet<HistoricalRATIShopper>("HistoricalRATIShoppers");
            builder.Entity<RATIShopper>().Collection.Action("ExportXLSX");
            builder.EntitySet<RATIShopper>("RATIShoppers").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<RATIShopper>("DeletedRATIShoppers").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.Entity<RATIShopper>().Collection.Action("FullImportXLSX");
            builder.Entity<RATIShopper>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<RATIShopper>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<RATIShopper>("RATIShoppers");
            builder.Entity<HistoricalRATIShopper>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalRATIShopper>("HistoricalRATIShoppers");

            builder.EntitySet<ActualTradeInvestment>("ActualTradeInvestments");
            builder.EntitySet<ActualTradeInvestment>("DeletedActualTradeInvestments");
            builder.EntitySet<HistoricalActualTradeInvestment>("HistoricalActualTradeInvestments");
            builder.Entity<ActualTradeInvestment>().Collection.Action("ExportXLSX");
            builder.EntitySet<ActualTradeInvestment>("ActualTradeInvestments").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualTradeInvestment>("DeletedActualTradeInvestments").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<ActualTradeInvestment>("ActualTradeInvestments").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<ActualTradeInvestment>("DeletedActualTradeInvestments").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<ActualTradeInvestment>().Collection.Action("FullImportXLSX");
            builder.Entity<ActualTradeInvestment>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<ActualTradeInvestment>().Collection.Action("IsTIRecalculatePreviousYearButtonAvailable");
            builder.Entity<ActualTradeInvestment>().Collection.Action("PreviousYearPromoList");
            builder.Entity<ActualTradeInvestment>().Collection.Action("CreateActualTIChangeIncidents");
            builder.Entity<ActualTradeInvestment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ActualTradeInvestment>("ActualTradeInvestments");
            builder.Entity<HistoricalActualTradeInvestment>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalActualTradeInvestment>("HistoricalActualTradeInvestments");

            builder.EntitySet<Plu>("Plus");
            builder.EntitySet<Plu>("Plus").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.Entity<Plu>().HasRequired(n => n.ClientTree, (n, p) => n.ClientTreeId == p.Id);

            builder.EntitySet<HistoricalPLUDictionary>("HistoricalPLUDictionaries");
            builder.Entity<HistoricalPLUDictionary>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalPLUDictionary>("HistoricalPLUDictionaries");


            builder.EntitySet<AssortmentMatrix>("AssortmentMatrices");
            builder.EntitySet<AssortmentMatrix>("DeletedAssortmentMatrices");
            builder.EntitySet<HistoricalAssortmentMatrix>("HistoricalAssortmentMatrices");
            builder.Entity<AssortmentMatrix>().Collection.Action("ExportXLSX");
            builder.EntitySet<AssortmentMatrix>("AssortmentMatrices").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<AssortmentMatrix>("DeletedAssortmentMatrices").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<AssortmentMatrix>("AssortmentMatrices").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<AssortmentMatrix>("DeletedAssortmentMatrices").HasRequiredBinding(e => e.Product, "Products");
            builder.Entity<AssortmentMatrix>().HasRequired(n => n.ClientTree, (n, p) => n.ClientTreeId == p.Id);
            builder.Entity<AssortmentMatrix>().HasRequired(n => n.Product, (n, p) => n.ProductId == p.Id);
            builder.Entity<AssortmentMatrix>().Collection.Action("FullImportXLSX");
            builder.Entity<AssortmentMatrix>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<AssortmentMatrix>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<AssortmentMatrix>("AssortmentMatrices");
            builder.Entity<HistoricalAssortmentMatrix>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalAssortmentMatrix>("HistoricalAssortmentMatrices");
            builder.EntitySet<AssortmentMatrix2Plu>("AssortmentMatrix2Plus");
            builder.Entity<AssortmentMatrix>().HasOptional(n => n.Plu, (n, p) => n.Id == p.Id);

            builder.Entity<PromoProductTree>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoProductTree>("PromoProductTrees").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.Entity<PromoUpliftFailIncident>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoUpliftFailIncident>("PromoUpliftFailIncidents").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.Entity<CurrentDayIncremental>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<CurrentDayIncremental>("CurrentDayIncrementals").HasRequiredBinding(g => g.Promo, "Promoes");
            builder.EntitySet<CurrentDayIncremental>("CurrentDayIncrementals").HasRequiredBinding(g => g.Product, "Products");

            builder.Entity<PromoOnApprovalIncident>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoOnApprovalIncident>("PromoOnApprovalIncidents").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.Entity<PromoOnRejectIncident>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoOnRejectIncident>("PromoOnRejectIncidents").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.Entity<PromoCancelledIncident>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoCancelledIncident>("PromoCancelledIncidents").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.Entity<PromoApprovedIncident>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);
            builder.EntitySet<PromoApprovedIncident>("PromoApprovedIncidents").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.EntitySet<PlanIncrementalReport>("PlanIncrementalReports");
            builder.Entity<PlanIncrementalReport>().Collection.Action("ExportXLSX");
            builder.Entity<PlanIncrementalReport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PlanIncrementalReport>("PlanIncrementalReports");

            builder.EntitySet<PlanPostPromoEffectReportWeekView>("PlanPostPromoEffectReports");
            builder.Entity<PlanPostPromoEffectReportWeekView>().Collection.Action("ExportXLSX");
            builder.Entity<PlanPostPromoEffectReportWeekView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PlanPostPromoEffectReportWeekView>("PlanPostPromoEffectReports");

            builder.EntitySet<PromoView>("PromoViews");
            builder.Entity<PromoView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoRSView>("PromoRSViews");
            builder.EntitySet<PromoRSView>("PromoRSViews");
            builder.Entity<PromoRSView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoRSView>("PromoRSViews");
            builder.EntitySet<PromoGridView>("PromoGridViews");
            builder.Entity<PromoGridView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoGridView>("PromoGridViews");
            builder.Entity<PromoGridView>().Collection.Action("ExportXLSX");
            builder.EntitySet<PlanIncrementalReport>("PlanIncrementalReports");

            //Загрузка шаблонов
            builder.Entity<BaseLine>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<IncreaseBaseLine>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Product>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Brand>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<NonPromoEquipment>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<NonPromoSupport>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Technology>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<BrandTech>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Budget>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<BudgetItem>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<BudgetSubItem>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Color>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Event>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<MechanicType>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoStatus>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<RejectReason>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Mechanic>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<NodeType>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<ClientTreeBrandTech>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<IncrementalPromo>("IncrementalPromoes");
            builder.EntitySet<IncrementalPromo>("IncrementalPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<IncrementalPromo>("IncrementalPromoes").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<HistoricalIncrementalPromo>("HistoricalIncrementalPromoes");
            builder.Entity<IncrementalPromo>().Collection.Action("ExportXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("FullImportXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<IncrementalPromo>("IncrementalPromoes");
            builder.Entity<HistoricalIncrementalPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalIncrementalPromo>("HistoricalIncrementalPromoes");

            builder.EntitySet<ActualLSV>("ActualLSVs");
            builder.Entity<ActualLSV>().Collection.Action("ExportXLSX");
            builder.Entity<ActualLSV>().Collection.Action("FullImportXLSX");
            builder.Entity<ActualLSV>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ActualLSV>("ActualLSVs");

            builder.EntitySet<PromoROIReport>("PromoROIReports");
            builder.Entity<PromoROIReport>().Collection.Action("ExportXLSX");
            builder.Entity<PromoROIReport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoROIReport>("PromoROIReports");

            builder.EntitySet<PromoPriceIncreaseROIReport>("PromoPriceIncreaseROIReports");
            builder.Entity<PromoPriceIncreaseROIReport>().Collection.Action("ExportXLSX");
            builder.Entity<PromoPriceIncreaseROIReport>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoPriceIncreaseROIReport>("PromoPriceIncreaseROIReports");

            builder.EntitySet<SchedulerClientTreeDTO>("SchedulerClientTreeDTOs");
            builder.Entity<SchedulerClientTreeDTO>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<SchedulerClientTreeDTO>("SchedulerClientTreeDTOs");

            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees");
            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees").HasRequiredBinding(e => e.BudgetSubItem, "BudgetSubItems");
            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.Entity<BudgetSubItemClientTree>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees");
            //builder.Entity<BudgetSubItemClientTree>().HasRequired(e => e.ClientTree, (e, te) => e.ClientTreeId == te.Id);
            //builder.Entity<BudgetSubItemClientTree>().HasRequired(e => e.BudgetSubItem, (e, te) => e.BudgetSubItemId == te.Id);

            builder.EntitySet<BTL>("BTLs");
            builder.EntitySet<BTL>("BTLs").HasOptionalBinding(e => e.Event, "Events");
            builder.EntitySet<BTL>("DeletedBTLs");
            builder.EntitySet<BTL>("DeletedBTLs").HasOptionalBinding(e => e.Event, "Events");
            builder.EntitySet<BTL>("BTLs").HasManyBinding(e => e.BTLPromoes, "BTLPromoes");
            builder.EntitySet<BTL>("DeletedBTLs").HasManyBinding(e => e.BTLPromoes, "BTLPromoes");
            builder.EntitySet<HistoricalBTL>("HistoricalBTLs");
            builder.Entity<BTL>().Collection.Action("ExportXLSX");
            builder.Entity<BTL>().Collection.Action("FullImportXLSX");
            builder.Entity<BTL>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BTL>("BTLs");
            builder.Entity<HistoricalBTL>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalBTL>("HistoricalBTLs");
            builder.Entity<BTL>().Collection.Action("GetEventBTL");

            builder.EntitySet<PriceList>("PriceLists");
            builder.Entity<PriceList>().Collection.Action("ExportXLSX");
            builder.EntitySet<PriceList>("PriceLists").HasRequiredBinding(p => p.Product, "Products");
            builder.EntitySet<PriceList>("PriceLists").HasRequiredBinding(p => p.ClientTree, "ClientTrees");
            builder.Entity<PriceList>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PriceList>("PriceLists");

            builder.EntitySet<CoefficientSI2SO>("CoefficientSI2SOs");
            builder.EntitySet<CoefficientSI2SO>("DeletedCoefficientSI2SOs");
            builder.EntitySet<HistoricalCoefficientSI2SO>("HistoricalCoefficientSI2SOs");
            builder.Entity<CoefficientSI2SO>().Collection.Action("ExportXLSX");
            builder.Entity<CoefficientSI2SO>().Collection.Action("FullImportXLSX");
            builder.Entity<CoefficientSI2SO>().Collection.Action("DownloadTemplateXLSX");
            builder.EntitySet<CoefficientSI2SO>("CoefficientSI2SOs").HasRequiredBinding(c => c.BrandTech, "BrandTeches");
            builder.EntitySet<CoefficientSI2SO>("DeletedCoefficientSI2SOs").HasRequiredBinding(c => c.BrandTech, "BrandTeches");
            builder.Entity<CoefficientSI2SO>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<CoefficientSI2SO>("CoefficientSI2SOs");
            builder.Entity<HistoricalCoefficientSI2SO>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCoefficientSI2SO>("HistoricalCoefficientSI2SOs");

            builder.EntitySet<BTLPromo>("BTLPromoes");
            builder.EntitySet<BTLPromo>("BTLPromoes").HasRequiredBinding(e => e.BTL, "BTLs");
            builder.EntitySet<BTLPromo>("BTLPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<BTLPromo>("BTLPromoes").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.Entity<BTLPromo>().Collection.Action("GetPromoesWithBTL").Parameter<string>("eventId");
            builder.Entity<BTLPromo>().Collection.Action("BTLPromoPost");
            builder.Entity<BTLPromo>().Collection.Action("BTLPromoDelete");
            builder.Entity<BTLPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<BTLPromo>("BTLPromoes");

            builder.EntitySet<ClientDashboard>("ClientDashboards");
            builder.Entity<ClientDashboard>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientDashboardView>("ClientDashboardViews");
            builder.EntitySet<ClientDashboardView>("ClientDashboardViews");
            builder.Entity<ClientDashboardView>().Collection.Action("Update");
            builder.Entity<ClientDashboardView>().Collection.Action("ExportXLSX");
            builder.Entity<ClientDashboardView>().Collection.Action("FullImportXLSX");
            builder.Entity<ClientDashboardView>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<ClientDashboardView>().Collection.Action("GetAllYEEF");
            builder.Entity<ClientDashboardView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientDashboardView>("ClientDashboardViews");
            builder.EntitySet<HistoricalClientDashboardView>("HistoricalClientDashboards");
            builder.Entity<HistoricalClientDashboardView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalClientDashboardView>("HistoricalClientDashboards");

            builder.EntitySet<ClientDashboardRSView>("ClientDashboardRSViews");
            builder.Entity<ClientDashboardRSView>().Collection.Action("ExportXLSX");
            builder.Entity<ClientDashboardRSView>().Collection.Action("GetAllYEEF");
            builder.Entity<ClientDashboardRSView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<ClientDashboardRSView>("ClientDashboardRSViews");

            // Calendar Competitors Entities
            builder.EntitySet<Competitor>("Competitors").HasManyBinding(g => g.CompetitorBrandTechs, "CompetitorBrandTechs");
            builder.EntitySet<Competitor>("DeletedCompetitors").HasManyBinding(g => g.CompetitorBrandTechs, "CompetitorBrandTechs");
            builder.EntitySet<Competitor>("Competitors").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<Competitor>("DeletedCompetitors").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<HistoricalCompetitor>("HistoricalCompetitors");
            builder.Entity<Competitor>().Collection.Action("ExportXLSX");
            builder.Entity<Competitor>().Collection.Action("FullImportXLSX");
            builder.Entity<Competitor>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Competitor>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<Competitor>("Competitors");
            builder.Entity<HistoricalCompetitor>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCompetitor>("HistoricalCompetitors");

            builder.EntitySet<CompetitorBrandTech>("CompetitorBrandTechs").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<CompetitorBrandTech>("DeletedCompetitorBrandTechs").HasManyBinding(g => g.CompetitorPromoes, "CompetitorPromoes");
            builder.EntitySet<HistoricalCompetitorBrandTech>("HistoricalCompetitorBrandTechs");
            builder.EntitySet<CompetitorBrandTech>("CompetitorBrandTechs").HasOptionalBinding(e => e.Competitor, "Competitors");
            builder.EntitySet<CompetitorBrandTech>("DeletedCompetitorBrandTechs").HasOptionalBinding(e => e.Competitor, "Competitors");
            builder.Entity<CompetitorBrandTech>().Collection.Action("FullImportXLSX");
            builder.Entity<CompetitorBrandTech>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<CompetitorBrandTech>().Collection.Action("ExportXLSX");
            builder.Entity<CompetitorBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<CompetitorBrandTech>("CompetitorBrandTechs");
            builder.Entity<HistoricalCompetitorBrandTech>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCompetitorBrandTech>("HistoricalCompetitorBrandTechs");

            builder.EntitySet<CompetitorPromo>("CompetitorPromoes");
            builder.EntitySet<CompetitorPromo>("DeletedCompetitorPromoes");
            builder.EntitySet<HistoricalCompetitorPromo>("HistoricalCompetitorPromoes");
            builder.EntitySet<CompetitorPromo>("CompetitorPromoes").HasOptionalBinding(e => e.Competitor, "Competitors");
            builder.EntitySet<CompetitorPromo>("DeletedCompetitorPromoes").HasOptionalBinding(e => e.Competitor, "Competitors");
            builder.EntitySet<CompetitorPromo>("CompetitorPromoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<CompetitorPromo>("DeletedCompetitorPromoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<CompetitorPromo>("CompetitorPromoes").HasOptionalBinding(e => e.CompetitorBrandTech, "CompetitorBrandTechs");
            builder.EntitySet<CompetitorPromo>("DeletedCompetitorPromoes").HasOptionalBinding(e => e.CompetitorBrandTech, "CompetitorBrandTechs");
            builder.Entity<CompetitorPromo>().Collection.Action("FullImportXLSX");
            builder.Entity<CompetitorPromo>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<CompetitorPromo>().Collection.Action("ExportXLSX");
            builder.Entity<CompetitorPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<CompetitorPromo>("CompetitorPromoes");
            builder.Entity<CompetitorPromo>().Collection.Action("NewFullImportXLSX");
            builder.Entity<HistoricalCompetitorPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCompetitorPromo>("HistoricalCompetitorPromoes");

            builder.EntitySet<RPASetting>("RPASettings");
            builder.EntitySet<RPA>("RPAs");
            builder.Entity<RPA>().Collection.Action("UploadFile");
            builder.Entity<RPA>().Collection.Action("SaveRPA");
            builder.Entity<RPA>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<RollingScenario>("RollingScenarios").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<RollingScenario>("DeletedRollingScenarios").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<RollingScenario>("RollingScenarios").HasManyBinding(e => e.Promoes, "Promoes");
            builder.EntitySet<RollingScenario>("DeletedRollingScenarios").HasManyBinding(e => e.Promoes, "Promoes");
            builder.EntitySet<RollingScenario>("RollingScenarios").HasManyBinding(e => e.SavedScenarios, "SavedScenarios");
            builder.EntitySet<RollingScenario>("DeletedRollingScenarios").HasManyBinding(e => e.SavedScenarios, "SavedScenarios");
            builder.Entity<RollingScenario>().Collection.Action("OnApproval");
            builder.Entity<RollingScenario>().Collection.Action("Approve");
            builder.Entity<RollingScenario>().Collection.Action("Decline");
            builder.Entity<RollingScenario>().Collection.Action("Calculate");
            builder.Entity<RollingScenario>().Collection.Action("GetVisibleButton");
            builder.Entity<RollingScenario>().Collection.Action("GetCanceled");
            builder.Entity<RollingScenario>().Collection.Action("MassApprove");
            builder.Entity<RollingScenario>().Collection.Action("UploadScenario");
            builder.Entity<RollingScenario>().Collection.Action("GetStatusScenario");
            builder.Entity<HistoricalCompetitorPromo>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<HistoricalCompetitorPromo>("HistoricalCompetitorPromoes");

            builder.EntitySet<PromoProductCorrectionView>("PromoProductCorrectionViews");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProductCorrectionView>("PromoProductCorrectionViews");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("ExportCorrectionXLSX");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProductCorrectionView>().Collection.Action("PromoProductCorrectionDelete");

            builder.EntitySet<PromoProductCorrectionPriceIncreaseView>("PromoProductCorrectionPriceIncreaseViews");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProductCorrectionPriceIncreaseView>("PromoProductCorrectionPriceIncreaseViews");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("ExportCorrectionXLSX");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProductCorrectionPriceIncreaseView>().Collection.Action("PromoProductCorrectionDelete");

            builder.EntitySet<PromoPriceIncrease>("PromoPriceIncreases").HasRequiredBinding(g => g.Promo, "Promoes");

            builder.EntitySet<PromoProductPriceIncrease>("PromoProductPriceIncreases").HasRequiredBinding(g => g.PromoProduct, "PromoProducts");
            builder.EntitySet<PromoProductPriceIncrease>("PromoProductPriceIncreases").HasRequiredBinding(g => g.PromoPriceIncrease, "PromoPriceIncreases");
            builder.EntitySet<PromoProductPriceIncrease>("PromoProductPriceIncreases").HasManyBinding(g => g.ProductCorrectionPriceIncreases, "PromoProductCorrectionPriceIncreases");
            builder.Entity<PromoProductPriceIncrease>().Collection.Action("ExportXLSX");

            builder.EntitySet<PromoProductCorrectionPriceIncrease>("PromoProductCorrectionPriceIncreases");

            builder.EntitySet<PromoProductPriceIncreasesView>("PromoProductPriceIncreaseViews");
            builder.Entity<PromoProductPriceIncreasesView>().Collection.Action("ExportXLSX");
            builder.Entity<PromoProductPriceIncreasesView>().Collection.Action("FullImportXLSX");
            builder.Entity<PromoProductPriceIncreasesView>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<PromoProductPriceIncreasesView>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<PromoProductPriceIncreasesView>("PromoProductPriceIncreaseViews");

            builder.EntitySet<MetricsLiveHistory>("MetricsLiveHistories");
            builder.Entity<MetricsLiveHistory>().Collection.Action("GetFilteredData").ReturnsCollectionFromEntitySet<MetricsLiveHistory>("MetricsLiveHistories");

            builder.EntitySet<SavedScenario>("SavedScenarios").HasRequiredBinding(e => e.RollingScenario, "RollingScenarios");
            builder.EntitySet<SavedScenario>("DeletedSavedScenarios").HasRequiredBinding(e => e.RollingScenario, "DeletedRollingScenarios");
            builder.EntitySet<SavedScenario>("SavedScenarios").HasManyBinding(e => e.Promoes, "Promoes");
            builder.EntitySet<SavedScenario>("DeletedSavedScenarios").HasManyBinding(e => e.Promoes, "DeletedPromoes");
            builder.Entity<SavedScenario>().Collection.Action("UploadSavedScenario");

            builder.EntitySet<SavedPromo>("SavedPromoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<SavedPromo>("DeletedSavedPromoes").HasOptionalBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<SavedPromo>("SavedPromoes").HasManyBinding(e => e.Promoes, "Promoes");
            builder.EntitySet<SavedPromo>("DeletedSavedPromoes").HasManyBinding(e => e.Promoes, "DeletedPromoes");
        }



        public IEnumerable<Type> GetHistoricalEntities()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseHistoricalEntity<Guid>)) && type.GetCustomAttribute<AssociatedWithAttribute>() != null);
        }
    }
}