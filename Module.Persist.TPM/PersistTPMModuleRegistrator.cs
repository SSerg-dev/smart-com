using Core.History;
using Core.ModuleRegistrator;
using Module.Persist.TPM.Model.History;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Http.OData.Builder;

namespace Module.Persist.TPM {
    public class PersistTPMModuleRegistrator : IPersistModuleRegistrator {
        public void ConfigurateDBModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Brand>();
            modelBuilder.Entity<Segment>();
            modelBuilder.Entity<Technology>();
            modelBuilder.Entity<TechHighLevel>();
            modelBuilder.Entity<Program>();
            modelBuilder.Entity<Format>();
            modelBuilder.Entity<BrandTech>();
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
            modelBuilder.Entity<Promo>();
            modelBuilder.Entity<Sale>();
            modelBuilder.Entity<Color>();
            modelBuilder.Entity<PromoSales>();
            modelBuilder.Entity<Demand>();
            modelBuilder.Entity<RejectReason>();
            modelBuilder.Entity<ClientTree>();
            modelBuilder.Entity<ProductTree>();
            modelBuilder.Entity<Event>();
            modelBuilder.Entity<NodeType>();
            modelBuilder.Entity<PromoStatusChange>();
            modelBuilder.Entity<PromoDemand>();
            modelBuilder.Entity<BaseClientTreeView>().ToTable("BaseClientTreeView");
            modelBuilder.Entity<RetailType>();
            modelBuilder.Entity<NoneNego>();
            modelBuilder.Entity<PromoProduct>();
            modelBuilder.Entity<BaseLine>();
            modelBuilder.Entity<ClientTreeSharesView>().ToTable("ClientTreeSharesView");
            modelBuilder.Entity<ProductChangeIncident>();
            modelBuilder.Entity<ClientTreeHierarchyView>();
            modelBuilder.Entity<ProductTreeHierarchyView>();
            modelBuilder.Entity<PromoDemandChangeIncident>();
            modelBuilder.Entity<BudgetSubItem>();
            modelBuilder.Entity<PromoSupport>().HasMany(p => p.PromoSupportPromo)
                .WithRequired(p => p.PromoSupport);
            modelBuilder.Entity<PostPromoEffect>();
            modelBuilder.Entity<PromoSupportPromo>();
            modelBuilder.Entity<PromoProductTree>();
            modelBuilder.Entity<COGS>();
            modelBuilder.Entity<TradeInvestment>();
            modelBuilder.Entity<PromoUpliftFailIncident>();
            modelBuilder.Entity<BlockedPromo>();
            modelBuilder.Entity<AssortmentMatrix>();
            modelBuilder.Entity<IncrementalPromo>();
            modelBuilder.Entity<PromoView>();
            modelBuilder.Entity<PromoGridView>();
            modelBuilder.Entity<PlanIncrementalReport>();
			modelBuilder.Entity<PromoCancelledIncident>();
            modelBuilder.Entity<ChangesIncident>();
			modelBuilder.Entity<PromoOnApprovalIncident>();
			modelBuilder.Entity<PromoOnRejectIncident>();
            modelBuilder.Entity<PromoApprovedIncident>();
            modelBuilder.Entity<EventClientTree>();
            modelBuilder.Entity<BudgetSubItemClientTree>();

            modelBuilder.Entity<Promo>().Ignore(n => n.ProductTreeObjectIds);
            modelBuilder.Entity<Promo>().Ignore(n => n.Calculating);
            modelBuilder.Entity<Promo>().Ignore(n => n.PromoBasicProducts);            
        }

        public void BuildEdm(ODataConventionModelBuilder builder) {
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Category>("DeletedCategories");
            builder.EntitySet<HistoricalCategory>("HistoricalCategories");
            builder.Entity<Category>().Collection.Action("ExportXLSX");

            builder.EntitySet<Brand>("Brands");
            builder.EntitySet<Brand>("DeletedBrands");
            builder.EntitySet<HistoricalBrand>("HistoricalBrands");
            builder.Entity<Brand>().Collection.Action("ExportXLSX");
            builder.Entity<Brand>().Collection.Action("FullImportXLSX");

            builder.EntitySet<Segment>("Segments");
            builder.EntitySet<Segment>("DeletedSegments");
            builder.EntitySet<HistoricalSegment>("HistoricalSegments");
            builder.Entity<Segment>().Collection.Action("ExportXLSX");

            builder.EntitySet<Event>("Events");
            builder.EntitySet<Event>("DeletedEvents");
            builder.EntitySet<HistoricalEvent>("HistoricalEvents");
            builder.Entity<Event>().Collection.Action("ExportXLSX");
            builder.Entity<Event>().Collection.Action("FullImportXLSX");

            builder.EntitySet<EventClientTree>("EventClientTrees");
            builder.EntitySet<EventClientTree>("EventClientTrees").HasRequiredBinding(e => e.ClientTree,"ClientTrees");
            builder.EntitySet<EventClientTree>("EventClientTrees").HasRequiredBinding(e => e.Event, "Events");

            builder.EntitySet<Technology>("Technologies");
            builder.EntitySet<Technology>("DeletedTechnologies");
            builder.EntitySet<HistoricalTechnology>("HistoricalTechnologies");
            builder.Entity<Technology>().Collection.Action("ExportXLSX");
            builder.Entity<Technology>().Collection.Action("FullImportXLSX");

            builder.EntitySet<TechHighLevel>("TechHighLevels");
            builder.EntitySet<TechHighLevel>("DeletedTechHighLevels");
            builder.EntitySet<HistoricalTechHighLevel>("HistoricalTechHighLevels");
            builder.Entity<TechHighLevel>().Collection.Action("ExportXLSX");

            builder.EntitySet<Program>("Programs");
            builder.EntitySet<Program>("DeletedPrograms");
            builder.EntitySet<HistoricalProgram>("HistoricalPrograms");
            builder.Entity<Program>().Collection.Action("ExportXLSX");

            builder.EntitySet<Format>("Formats");
            builder.EntitySet<Format>("DeletedFormats");
            builder.EntitySet<HistoricalFormat>("HistoricalFormats");
            builder.Entity<Format>().Collection.Action("ExportXLSX");

            builder.EntitySet<BrandTech>("BrandTeches");
            builder.EntitySet<BrandTech>("DeletedBrandTeches");
            builder.EntitySet<HistoricalBrandTech>("HistoricalBrandTeches");
            builder.EntitySet<BrandTech>("BrandTeches").HasRequiredBinding(e => e.Brand, "Brands");
            builder.EntitySet<BrandTech>("BrandTeches").HasRequiredBinding(e => e.Technology, "Technologies");            
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasRequiredBinding(e => e.Brand, "Brands");            
            builder.EntitySet<BrandTech>("DeletedBrandTeches").HasRequiredBinding(e => e.Technology, "Technologies");
            builder.Entity<BrandTech>().HasRequired(n => n.Brand, (n, b) => n.BrandId == b.Id);
            builder.Entity<BrandTech>().HasRequired(n => n.Technology, (n, t) => n.TechnologyId == t.Id);
            builder.Entity<BrandTech>().Collection.Action("ExportXLSX");
            builder.Entity<BrandTech>().Collection.Action("FullImportXLSX");

            builder.EntitySet<Subrange>("Subranges");
            builder.EntitySet<Subrange>("DeletedSubranges");
            builder.EntitySet<HistoricalSubrange>("HistoricalSubranges");
            builder.Entity<Subrange>().Collection.Action("ExportXLSX");

            builder.EntitySet<AgeGroup>("AgeGroups");
            builder.EntitySet<AgeGroup>("DeletedAgeGroups");
            builder.EntitySet<HistoricalAgeGroup>("HistoricalAgeGroups");
            builder.Entity<AgeGroup>().Collection.Action("ExportXLSX");

            builder.EntitySet<Variety>("Varieties");
            builder.EntitySet<Variety>("DeletedVarieties");
            builder.EntitySet<HistoricalVariety>("HistoricalVarieties");
            builder.Entity<Variety>().Collection.Action("ExportXLSX");

            builder.EntitySet<Mechanic>("Mechanics");
            builder.EntitySet<Mechanic>("DeletedMechanics");
            builder.EntitySet<HistoricalMechanic>("HistoricalMechanics");
            builder.Entity<Mechanic>().Collection.Action("ExportXLSX");
            builder.Entity<Mechanic>().Collection.Action("FullImportXLSX");

            builder.EntitySet<MechanicType>("MechanicTypes");
            builder.EntitySet<MechanicType>("DeletedMechanicTypes");
            builder.EntitySet<HistoricalMechanicType>("HistoricalMechanicTypes");
            builder.Entity<MechanicType>().Collection.Action("ExportXLSX");
            builder.Entity<MechanicType>().Collection.Action("FullImportXLSX");

            builder.EntitySet<PromoStatus>("PromoStatuss");
            builder.EntitySet<PromoStatus>("DeletedPromoStatuss");
            builder.EntitySet<HistoricalPromoStatus>("HistoricalPromoStatuss");
            builder.Entity<PromoStatus>().Collection.Action("ExportXLSX");
            builder.Entity<PromoStatus>().Collection.Action("FullImportXLSX");

            builder.EntitySet<RejectReason>("RejectReasons");
            builder.EntitySet<RejectReason>("DeletedRejectReasons");
            builder.EntitySet<HistoricalRejectReason>("HistoricalRejectReasons");
            builder.Entity<RejectReason>().Collection.Action("ExportXLSX");
            builder.Entity<RejectReason>().Collection.Action("FullImportXLSX");

            builder.EntitySet<Product>("Products");
            builder.EntitySet<Product>("DeletedProducts");
            builder.EntitySet<HistoricalProduct>("HistoricalProducts");
            builder.Entity<Product>().Collection.Action("ExportXLSX");
            builder.Entity<Product>().Collection.Action("FullImportXLSX");
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

            builder.EntitySet<CommercialNet>("CommercialNets");
            builder.EntitySet<CommercialNet>("DeletedCommercialNets");
            builder.EntitySet<HistoricalCommercialNet>("HistoricalCommercialNets");
            builder.Entity<CommercialNet>().Collection.Action("ExportXLSX");

            builder.EntitySet<CommercialSubnet>("CommercialSubnets");
            builder.EntitySet<CommercialSubnet>("DeletedCommercialSubnets");
            builder.EntitySet<HistoricalCommercialSubnet>("HistoricalCommercialSubnets");
            builder.EntitySet<CommercialSubnet>("CommercialSubnets").HasRequiredBinding(e => e.CommercialNet, "CommercialNets");
            builder.Entity<CommercialSubnet>().HasRequired(e => e.CommercialNet, (e, te) => e.CommercialNetId == te.Id);
            builder.EntitySet<CommercialSubnet>("DeletedCommercialSubnets").HasRequiredBinding(e => e.CommercialNet, "CommercialNets");
            builder.Entity<CommercialSubnet>().Collection.Action("ExportXLSX");

            builder.EntitySet<Distributor>("Distributors");
            builder.EntitySet<Distributor>("DeletedDistributors");
            builder.EntitySet<HistoricalDistributor>("HistoricalDistributors");
            builder.Entity<Distributor>().Collection.Action("ExportXLSX");

            builder.EntitySet<StoreType>("StoreTypes");
            builder.EntitySet<StoreType>("DeletedStoreTypes");
            builder.EntitySet<HistoricalStoreType>("HistoricalStoreTypes");
            builder.Entity<StoreType>().Collection.Action("ExportXLSX");

            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Client>("DeletedClients");
            builder.EntitySet<HistoricalClient>("HistoricalClients");
            builder.EntitySet<Client>("Clients").HasRequiredBinding(e => e.CommercialSubnet, "CommercialSubnets");
            builder.Entity<Client>().HasRequired(e => e.CommercialSubnet, (e, te) => e.CommercialSubnetId == te.Id);
            builder.EntitySet<Client>("DeletedClients").HasRequiredBinding(e => e.CommercialSubnet, "CommercialSubnets");
            builder.Entity<Client>().Collection.Action("ExportXLSX");

            builder.EntitySet<Budget>("Budgets");
            builder.EntitySet<Budget>("DeletedBudgets");
            builder.EntitySet<HistoricalBudget>("HistoricalBudgets");
            builder.Entity<Budget>().Collection.Action("ExportXLSX");
            builder.Entity<Budget>().Collection.Action("FullImportXLSX");

            builder.EntitySet<BudgetItem>("BudgetItems");
            builder.EntitySet<BudgetItem>("DeletedBudgetItems");
            builder.EntitySet<HistoricalBudgetItem>("HistoricalBudgetItems");
            builder.EntitySet<BudgetItem>("BudgetItems").HasRequiredBinding(e => e.Budget, "Budgets");
            builder.Entity<BudgetItem>().HasRequired(e => e.Budget, (e, te) => e.BudgetId == te.Id);
            builder.EntitySet<BudgetItem>("DeletedBudgetItems").HasRequiredBinding(e => e.Budget, "Budgets");
            builder.Entity<BudgetItem>().Collection.Action("ExportXLSX");
            builder.Entity<BudgetItem>().Collection.Action("FullImportXLSX");

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
            builder.Entity<Promo>().Collection.Action("ExportXLSX");
            builder.Entity<Promo>().Collection.Action("FullImportXLSX");
            builder.Entity<Promo>().Collection.Action("DeclinePromo");
            builder.Entity<Promo>().Collection.Action("GetApprovalHistory");
            builder.Entity<Promo>().Collection.Action("CalculateMarketingTI");
            builder.Entity<Promo>().Collection.Action("ChangeStatus");
            builder.Entity<Promo>().Collection.Action("ExportPromoROIReportXLSX");
            builder.Entity<Promo>().Collection.Action("RecalculatePromo");
            builder.Entity<Promo>().Collection.Action("CheckIfLogHasErrors");
			builder.Entity<Promo>().Collection.Action("CheckPromoCreator");
			builder.Entity<Promo>().Collection.Action("GetProducts").CollectionParameter<string>("InOutProductIds");

            ActionConfiguration schedExp = builder.Entity<Promo>().Collection.Action("ExportSchedule");
            schedExp.CollectionParameter<int>("clients");
            schedExp.Parameter<int?>("year");

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

            //builder.EntitySet<DemandDTO>("DemandDTOs");
            //builder.Entity<DemandDTO>().Collection.Action("FullImportXLSX");
            //builder.EntitySet<DemandDTO>("DemandDTOs").HasOptionalBinding(e => e.Brand, "Brands");

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

            builder.EntitySet<Color>("Colors");
            builder.EntitySet<Color>("DeletedColors");
            builder.EntitySet<HistoricalColor>("HistoricalColors");
            builder.EntitySet<Color>("Colors").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.EntitySet<Color>("DeletedColors").HasOptionalBinding(e => e.BrandTech, "BrandTeches");
            builder.Entity<Color>().Collection.Action("GetSuitable");
            builder.Entity<Color>().Collection.Action("FullImportXLSX");
            builder.Entity<Color>().Collection.Action("ExportXLSX");

            builder.EntitySet<ClientTree>("ClientTrees");
            builder.EntitySet<ClientTree>("BaseClients"); // Для получение только базовых клиентов из иерархии
            builder.Entity<ClientTree>().Collection.Action("Delete");
            builder.Entity<ClientTree>().Collection.Action("Move");
            ActionConfiguration updateClientNodeAction = builder.Entity<ClientTree>().Collection.Action("UpdateNode");
            updateClientNodeAction.ReturnsFromEntitySet<ClientTree>("ClientTrees"); //какого типа параметр принимает экшн.
            builder.Entity<ClientTree>().Collection.Action("CanCreateBaseClient");
            builder.Entity<ClientTree>().Collection.Action("UploadLogoFile");
            builder.Entity<ClientTree>().Collection.Action("DownloadLogoFile");
            builder.Entity<ClientTree>().Collection.Action("DeleteLogo");
            builder.Entity<BaseClientTreeView>().Collection.Action("ExportXLSX");

            builder.EntitySet<ProductTree>("ProductTrees");
            builder.Entity<ProductTree>().Collection.Action("Delete");
            builder.Entity<ProductTree>().Collection.Action("Move");
            builder.Entity<ProductTree>().Collection.Action("UploadLogoFile");
            builder.Entity<ProductTree>().Collection.Action("DownloadLogoFile");
            builder.Entity<ProductTree>().Collection.Action("DeleteLogo");
            ActionConfiguration updateProductNodeAction = builder.Entity<ProductTree>().Collection.Action("UpdateNode");
            updateProductNodeAction.ReturnsFromEntitySet<ProductTree>("ProductTrees"); //какого типа параметр принимает экшн.

            builder.EntitySet<NodeType>("NodeTypes");
            builder.EntitySet<NodeType>("DeletedNodeTypes");
            builder.EntitySet<HistoricalNodeType>("HistoricalNodeTypes");
            builder.Entity<NodeType>().Collection.Action("FullImportXLSX");
            builder.Entity<NodeType>().Collection.Action("ExportXLSX");

            builder.EntitySet<PromoStatusChange>("PromoStatusChanges");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.PromoStatus, "PromoStatuss");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.Promo, "Promoes");
            builder.EntitySet<PromoStatusChange>("PromoStatusChanges").HasOptionalBinding(e => e.RejectReason, "RejectReasons");
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("UserName"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("RoleName"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("StatusColor"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(PromoStatusChange)).AddProperty(typeof(PromoStatusChange).GetProperty("StatusName"));
            builder.Entity<PromoStatusChange>().Collection.Action("PromoStatusChangesByPromo");

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

            builder.EntitySet<BaseClientTreeView>("BaseClientTreeViews");

            builder.EntitySet<ClientTreeSharesView>("ClientTreeSharesViews");
            builder.Entity<ClientTreeSharesView>().Collection.Action("ExportXLSX");
            builder.Entity<ClientTreeSharesView>().Collection.Action("FullImportXLSX");

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

            builder.EntitySet<PromoProduct>("PromoProducts");
            builder.EntitySet<PromoProduct>("DeletedPromoProducts");
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
            builder.Entity<PromoProduct>().Collection.Action("DownloadTemplateXLSXTLC");
            builder.Entity<PromoProduct>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<BaseLine>("BaseLines");
            builder.EntitySet<BaseLine>("DeletedBaseLines");
            builder.EntitySet<BaseLine>("BaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<BaseLine>("DeletedBaseLines").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<HistoricalBaseLine>("HistoricalBaseLines");
            builder.Entity<BaseLine>().Collection.Action("ExportXLSX");
            builder.Entity<BaseLine>().Collection.Action("ExportDemandPriceListXLSX");
            builder.Entity<BaseLine>().Collection.Action("FullImportXLSX");

            builder.EntitySet<RetailType>("RetailTypes");
            builder.EntitySet<RetailType>("DeletedRetailTypes");
            builder.EntitySet<HistoricalRetailType>("HistoricalRetailTypes");
            builder.Entity<RetailType>().Collection.Action("ExportXLSX");
            builder.Entity<RetailType>().Collection.Action("FullImportXLSX");
            builder.Entity<RetailType>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<ProductChangeIncident>("ProductChangeIncidents").HasRequiredBinding(e => e.Product, "Products");
            builder.Entity<ProductChangeIncident>().HasRequired(e => e.Product, (e, te) => e.ProductId == te.Id);

            builder.EntitySet<BudgetSubItem>("BudgetSubItems");
            builder.EntitySet<BudgetSubItem>("DeletedBudgetSubItems");
            builder.EntitySet<HistoricalBudgetSubItem>("HistoricalBudgetSubItems");
            builder.EntitySet<BudgetSubItem>("BudgetSubItems").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.EntitySet<BudgetSubItem>("DeletedBudgetSubItems").HasRequiredBinding(e => e.BudgetItem, "BudgetItems");
            builder.Entity<BudgetSubItem>().HasRequired(e => e.BudgetItem, (e, bi) => e.BudgetItemId == bi.Id);            
            builder.Entity<BudgetSubItem>().Collection.Action("ExportXLSX");
            builder.Entity<BudgetSubItem>().Collection.Action("FullImportXLSX");

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
            builder.EntitySet<PostPromoEffect>("PostPromoEffects");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects");
            builder.EntitySet<HistoricalPostPromoEffect>("HistoricalPostPromoEffects");            
            builder.EntitySet<PostPromoEffect>("PostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PostPromoEffect>("PostPromoEffects").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            builder.EntitySet<PostPromoEffect>("DeletedPostPromoEffects").HasRequiredBinding(e => e.ProductTree, "ProductTrees");
            builder.Entity<PostPromoEffect>().Collection.Action("ExportXLSX");

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

            builder.Entity<PromoProductTree>().HasRequired(n => n.Promo, (n, p) => n.PromoId == p.Id);

            builder.EntitySet<PlanIncrementalReport>("PlanIncrementalReports");
            builder.Entity<PlanIncrementalReport>().Collection.Action("ExportXLSX");

            builder.EntitySet<PlanPostPromoEffectReportWeekView>("PlanPostPromoEffectReports");
            builder.Entity<PlanPostPromoEffectReportWeekView>().Collection.Action("ExportXLSX");

            builder.Entity<Promo>().Collection.Action("ReadPromoCalculatingLog");
            builder.Entity<Promo>().Collection.Action("GetHandlerIdForBlockedPromo");

            builder.EntitySet<PromoView>("PromoViews");
            builder.EntitySet<PromoGridView>("PromoGridViews");
            builder.Entity<PromoGridView>().Collection.Action("ExportXLSX");
            builder.EntitySet<PlanIncrementalReport>("PlanIncrementalReports");

            //Загрузка шаблонов
            builder.Entity<BaseLine>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Product>().Collection.Action("DownloadTemplateXLSX");
            builder.Entity<Brand>().Collection.Action("DownloadTemplateXLSX");
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
            builder.Entity<ClientTreeSharesView>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<IncrementalPromo>("IncrementalPromoes");
            builder.EntitySet<IncrementalPromo>("IncrementalPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<IncrementalPromo>("IncrementalPromoes").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes").HasRequiredBinding(e => e.Promo, "Promoes");
            builder.EntitySet<IncrementalPromo>("DeletedIncrementalPromoes").HasRequiredBinding(e => e.Product, "Products");
            builder.EntitySet<HistoricalIncrementalPromo>("HistoricalIncrementalPromoes");
            builder.Entity<IncrementalPromo>().Collection.Action("ExportXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("ExportDemandPriceListXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("FullImportXLSX");
            builder.Entity<IncrementalPromo>().Collection.Action("DownloadTemplateXLSX");

            builder.EntitySet<ActualLSV>("ActualLSVs");
            builder.Entity<ActualLSV>().Collection.Action("ExportXLSX");
            builder.Entity<ActualLSV>().Collection.Action("FullImportXLSX");

            builder.EntitySet<PromoROIReport>("PromoROIReports");
            builder.Entity<PromoROIReport>().Collection.Action("ExportXLSX");

            builder.EntitySet<SchedulerClientTreeDTO>("SchedulerClientTreeDTOs");

            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees");
            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees").HasRequiredBinding(e => e.BudgetSubItem, "BudgetSubItems");
            builder.EntitySet<BudgetSubItemClientTree>("BudgetSubItemClientTrees").HasRequiredBinding(e => e.ClientTree, "ClientTrees");
            //builder.Entity<BudgetSubItemClientTree>().HasRequired(e => e.ClientTree, (e, te) => e.ClientTreeId == te.Id);
            //builder.Entity<BudgetSubItemClientTree>().HasRequired(e => e.BudgetSubItem, (e, te) => e.BudgetSubItemId == te.Id);

            ActionConfiguration getSelectedProductsAction = builder.Entity<Product>().Collection.Action("GetSelectedProducts");
			getSelectedProductsAction.ReturnsCollectionFromEntitySet<IQueryable<Product>>("SelectedProducts");
			getSelectedProductsAction.CollectionParameter<string>("jsonData");

            ActionConfiguration getRegularSelectedProductsAction = builder.Entity<Product>().Collection.Action("GetRegularSelectedProducts");
			getRegularSelectedProductsAction.ReturnsCollectionFromEntitySet<IQueryable<Product>>("RegularSelectedProducts");
			getRegularSelectedProductsAction.CollectionParameter<string>("jsonData");
        }

        public IEnumerable<Type> GetHistoricalEntities() {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseHistoricalEntity<Guid>)) && type.GetCustomAttribute<AssociatedWithAttribute>() != null);
        }
    }
}