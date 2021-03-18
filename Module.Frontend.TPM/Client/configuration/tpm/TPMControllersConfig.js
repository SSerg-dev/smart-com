ResourceMgr.defineControllers('tpm', [
    'tpm.agegroup.AgeGroup',
    'tpm.agegroup.DeletedAgeGroup',
    'tpm.agegroup.HistoricalAgeGroup',

    'tpm.brand.Brand',
    'tpm.brand.DeletedBrand',
    'tpm.brand.HistoricalBrand',

    'tpm.brandtech.BrandTech',
    'tpm.brandtech.DeletedBrandTech',
    'tpm.brandtech.HistoricalBrandTech',

    'tpm.category.Category',
    'tpm.category.DeletedCategory',
    'tpm.category.HistoricalCategory',

    'tpm.client.Client',
    'tpm.client.DeletedClient',
    'tpm.client.HistoricalClient',

    'tpm.commercialnet.CommercialNet',
    'tpm.commercialnet.DeletedCommercialNet',
    'tpm.commercialnet.HistoricalCommercialNet',

    'tpm.commercialsubnet.CommercialSubnet',
    'tpm.commercialsubnet.DeletedCommercialSubnet',
    'tpm.commercialsubnet.HistoricalCommercialSubnet',

    'tpm.distributor.Distributor',
    'tpm.distributor.DeletedDistributor',
    'tpm.distributor.HistoricalDistributor',

    'tpm.format.Format',
    'tpm.format.DeletedFormat',
    'tpm.format.HistoricalFormat',

    'tpm.product.Product',
    'tpm.product.DeletedProduct',
    'tpm.product.HistoricalProduct',

    'tpm.program.Program',
    'tpm.program.DeletedProgram',
    'tpm.program.HistoricalProgram',

    'tpm.region.Region',
    'tpm.region.DeletedRegion',
    'tpm.region.HistoricalRegion',

    'tpm.segment.Segment',
    'tpm.segment.DeletedSegment',
    'tpm.segment.HistoricalSegment',

    'tpm.storetype.StoreType',
    'tpm.storetype.DeletedStoreType',
    'tpm.storetype.HistoricalStoreType',

    'tpm.subrange.Subrange',
    'tpm.subrange.DeletedSubrange',
    'tpm.subrange.HistoricalSubrange',

    'tpm.techhighlevel.TechHighLevel',
    'tpm.techhighlevel.DeletedTechHighLevel',
    'tpm.techhighlevel.HistoricalTechHighLevel',

    'tpm.technology.Technology',
    'tpm.technology.DeletedTechnology',
    'tpm.technology.HistoricalTechnology',

    'tpm.variety.Variety',
    'tpm.variety.DeletedVariety',
    'tpm.variety.HistoricalVariety',

    'tpm.mechanic.Mechanic',
    'tpm.mechanic.DeletedMechanic',
    'tpm.mechanic.HistoricalMechanic',

    'tpm.mechanictype.MechanicType',
    'tpm.mechanictype.DeletedMechanicType',
    'tpm.mechanictype.HistoricalMechanicType',

    'tpm.promostatus.PromoStatus',
    'tpm.promostatus.DeletedPromoStatus',
    'tpm.promostatus.HistoricalPromoStatus',

    'tpm.budget.Budget',
    'tpm.budget.DeletedBudget',
    'tpm.budget.HistoricalBudget',

    'tpm.budgetitem.BudgetItem',
    'tpm.budgetitem.DeletedBudgetItem',
    'tpm.budgetitem.HistoricalBudgetItem',

    'tpm.budgetsubitem.BudgetSubItem',
    'tpm.budgetsubitem.DeletedBudgetSubItem',
    'tpm.budgetsubitem.HistoricalBudgetSubItem',
    'tpm.budgetsubitem.BudgetSubItemClientTree',

    'tpm.promo.Promo',
    'tpm.promo.PromoChangeStatus',
    'tpm.promo.DeletedPromo',
    'tpm.promo.HistoricalPromo',

    'tpm.promo.UserRolePromo',
    'tpm.promo.HistoricalUserRolePromo',

    'tpm.promosupport.PromoSupport',
    'tpm.promosupport.DeletedPromoSupport',
	'tpm.promosupport.HistoricalPromoSupport',

	'tpm.nonpromosupport.NonPromoSupport',
	'tpm.nonpromosupport.DeletedNonPromoSupport',
	'tpm.nonpromosupport.HistoricalNonPromoSupport',
	'tpm.nonpromosupport.NonPromoSupportBrandTechChoose',

	'tpm.nonpromoequipment.NonPromoEquipment',
	'tpm.nonpromoequipment.DeletedNonPromoEquipment',
    'tpm.nonpromoequipment.HistoricalNonPromoEquipment',

    'tpm.nonpromosupportbrandtech.NonPromoSupportBrandTech',
    'tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail',

    'tpm.costproduction.CostProduction',
    'tpm.costproduction.DeletedCostProduction',
    'tpm.costproduction.HistoricalCostProduction',

    'tpm.promolinked.PromoLinked',

    'tpm.sale.Sale',
    'tpm.sale.DeletedSale',
    'tpm.sale.HistoricalSale',

    'tpm.schedule.SchedulerViewController',
    'tpm.schedule.ClientPromoTypeFilter',

    'tpm.demand.Demand',
    'tpm.demand.DeletedDemand',
    'tpm.demand.HistoricalDemand',

    'tpm.promosales.PromoSales',
    'tpm.promosales.DeletedPromoSales',
    'tpm.promosales.HistoricalPromoSales',

     //Color,
    'tpm.color.Color',
    'tpm.color.DeletedColor',
    'tpm.color.HistoricalColor',

    'tpm.rejectreason.RejectReason',
    'tpm.rejectreason.DeletedRejectReason',
    'tpm.rejectreason.HistoricalRejectReason',

    'tpm.event.Event',
    'tpm.event.DeletedEvent',
    'tpm.event.HistoricalEvent',
    'tpm.event.EventClientTree',

    'tpm.nodetype.NodeType',
    'tpm.nodetype.DeletedNodeType',
    'tpm.nodetype.HistoricalNodeType',

    'tpm.client.ClientTree',
    'tpm.product.ProductTree',

    'tpm.filter.Filter',

    //PromoDemand,
    'tpm.promodemand.PromoDemand',
    'tpm.promodemand.DeletedPromoDemand',
    'tpm.promodemand.HistoricalPromoDemand',

    //BaseClientTreeView
    'tpm.baseclienttreeview.BaseClientTreeView',

    'tpm.rollingvolume.RollingVolume',

    'tpm.nonenego.NoneNego',
    'tpm.nonenego.DeletedNoneNego',
    'tpm.nonenego.HistoricalNoneNego',

    'tpm.retailtype.RetailType',
    'tpm.retailtype.DeletedRetailType',
    'tpm.retailtype.HistoricalRetailType',

    //PromoProducts
    'tpm.promoproduct.PromoProduct',
    'tpm.promoproduct.DeletedPromoProduct',
    'tpm.promoproduct.HistoricalPromoProduct',

    //PromoProductsView
    'tpm.promoproductsview.PromoProductsView',

    //previousdayincremental
    'tpm.previousdayincremental.PreviousDayIncremental', 

    //promoproductcorrection
    'tpm.promoproductcorrection.PromoProductCorrection',
    'tpm.promoproductcorrection.DeletedPromoProductCorrection',
    'tpm.promoproductcorrection.HistoricalPromoProductCorrection',

    //BaseLine
    'tpm.baseline.BaseLine',
    'tpm.baseline.DeletedBaseLine',
    'tpm.baseline.HistoricalBaseLine',

    //ClientTreeBrandTech
    'tpm.clienttreebrandtech.ClientTreeBrandTech',
    'tpm.clienttreebrandtech.HistoricalClientTreeBrandTech',

    //ClientTreeSharesView
    'tpm.clienttreesharesview.ClientTreeSharesView',

    //PostPromoEffect
    'tpm.postpromoeffect.PostPromoEffect',
    'tpm.postpromoeffect.DeletedPostPromoEffect',
    'tpm.postpromoeffect.HistoricalPostPromoEffect',

    // PromoSupportPromo
    //'tpm.promosupportpromo.PromoSupportPromo',

    'tpm.cogs.COGS',
    'tpm.cogs.DeletedCOGS',
    'tpm.cogs.HistoricalCOGS',
    
    'tpm.tradeinvestment.DeletedTradeInvestment',
    'tpm.tradeinvestment.HistoricalTradeInvestment',
    'tpm.tradeinvestment.TradeInvestment',

    'tpm.actualcogs.ActualCOGS',
    'tpm.actualcogs.DeletedActualCOGS',
    'tpm.actualcogs.HistoricalActualCOGS',

    'tpm.actualtradeinvestment.ActualTradeInvestment',
    'tpm.actualtradeinvestment.DeletedActualTradeInvestment',
    'tpm.actualtradeinvestment.HistoricalActualTradeInvestment',

    'tpm.ratishopper.RATIShopper',
    'tpm.ratishopper.DeletedRATIShopper',
    'tpm.ratishopper.HistoricalRATIShopper',

    'tpm.actualproductsview.ActualProductsView',
    'tpm.planincrementalreport.PlanIncrementalReport',

    'tpm.promo.PromoBudgetDetails',
    'tpm.promosupportpromo.PSPshortFactCalculation',
    'tpm.promosupportpromo.PSPshortPlanCalculation',
    'tpm.promosupportpromo.PSPshortFactCostProd',
    'tpm.promosupportpromo.PSPshortPlanCostProd',
    'tpm.promosupport.PromoSupportChoose',

    'tpm.planpostpromoeffectreport.PlanPostPromoEffectReport',
    'tpm.promoroireport.PromoROIReport',

    // Promo Activity Details Info
    'tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo',

    'tpm.assortmentmatrix.AssortmentMatrix',
    'tpm.assortmentmatrix.DeletedAssortmentMatrix',
    'App.controller.tpm.assortmentmatrix.HistoricalAssortmentMatrix',

    'tpm.incrementalpromo.IncrementalPromo',
    'tpm.incrementalpromo.DeletedIncrementalPromo',
    'tpm.incrementalpromo.HistoricalIncrementalPromo',

    // Calculating info log
    'tpm.promocalculating.CalculatingInfoLog',

    //Actual LSV
    'tpm.actualLSV.ActualLSV',

    'tpm.promotypes.PromoTypes',
    'tpm.promotypes.DeletedPromoTypes',
    'tpm.promotypes.HistoricalPromoTypes',

    'tpm.pricelist.PriceList',

    'tpm.coefficientsi2so.CoefficientSI2SO',
    'tpm.coefficientsi2so.DeletedCoefficientSI2SO',
    'tpm.coefficientsi2so.HistoricalCoefficientSI2SO',

    'tpm.inoutselectionproductwindow.InOutSelectionProductWindow',

    'tpm.userdashboard.UserDashboard',
    'tpm.clientdashboard.ClientDashboard',

    'tpm.clientkpidata.ClientKPIData',
    'tpm.clientkpidata.HistoricalClientKPIData',

    // вспомогательный контроллер для промо
    'tpm.promo.PromoHelper',

    'tpm.btl.BTL',
    'tpm.btl.BTLPromo',
    'tpm.btl.DeletedBTL',
    'tpm.btl.HistoricalBTL',
]);
