MenuMgr.defineMenu([{
    text: l10n.ns('tpm', 'mainmenu').value('SchedulerItem'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('SchedulerItem'),
    glyph: 0xf0f1,
    scale: 'medium',
    widget: 'schedulecontainer'
}, {
    text: l10n.ns('tpm', 'mainmenu').value('PromoItem'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('PromoItem'),
    scale: 'medium',
    glyph: 0xf392,
    widget: 'promo'
}, {
    text: l10n.ns('tpm', 'mainmenu').value('ClientItem'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('ClientItem'),
    scale: 'medium',
    glyph: 0xf007,
    children: [{
        text: l10n.ns('tpm', 'mainmenu').value('ClientTreeItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ClientTreeItem'),
        glyph: 0xf007,
        
        widget: 'clienttree'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('BaseClientTreeView'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BaseClientTreeView'),
        glyph: 0xf007,
        widget: 'baseclienttreeview'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('RetailType'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('RetailType'),
        glyph: 0xf110,
        
        widget: 'retailtype'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('ClientTreeSharesView'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ClientTreeSharesView'),
        glyph: 0xf007,
        widget: 'clienttreesharesview'
    }]
}, {
    text: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
    scale: 'medium',
    glyph: 0xf1a6,
    children: [{
        text: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
        glyph: 0xf1a6,
        
        widget: 'product'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('ProductTreeItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ProductTreeItem'),
        glyph: 0xf645,
        
        widget: 'producttree'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('BrandItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BrandItem'),
        glyph: 0xf071,
        
        widget: 'brand'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('TechnologyItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('TechnologyItem'),
        glyph: 0xf3d3,
        
        widget: 'technology'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('BrandTechItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BrandTechItem'),
        glyph: 0xf619,
        
        widget: 'brandtech'
    }]
}, {
    text: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
    scale: 'medium',
    glyph: 0xf1b3,
    children: [{
        text: l10n.ns('tpm', 'mainmenu').value('TICosts'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
        glyph: 0xf1b3,
        //roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager', 'SuperReader'],
        widget: 'associatedpromosupport',
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
        glyph: 0xf1b3,
        //roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'SuperReader'],
        widget: 'associatedcostproduction'
    }]
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('DemandItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('DemandItem'),
        scale: 'medium',
        glyph: 0xf127,
        children: [/*{
            text: l10n.ns('tpm', 'mainmenu').value('ActualItem'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('ActualItem'),
            glyph: 0xf127,
            scale: 'medium',
            roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning'],
            widget: 'actual'
        }, */{
            text: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
            glyph: 0xf127,
            scale: 'medium',
            //roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
            widget: 'baseline'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('DemandPriceListItem'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('DemandPriceListItem'),
            glyph: 0xf127,
            scale: 'medium',
            //roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
            widget: 'demandpricelist'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
            glyph: 0xf127,
            scale: 'medium',
            roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader'],
            widget: 'planincrementalreport'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectReport'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectReport'),
            glyph: 0xf127,
            scale: 'medium',
            roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader'],
            widget: 'planpostpromoeffectreport'
        }]
    }, {
    text: l10n.ns('tpm', 'mainmenu').value('TablesItem'),
    tooltip: l10n.ns('tpm', 'mainmenu').value('TablesItem'),
    scale: 'medium',
    glyph: 0xf0ba,
    children: [{
        text: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
        glyph: 0xf218,
        widget: 'budget'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
        glyph: 0xf0be,
        widget: 'budgetitem'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
        glyph: 0xf0be,
        widget: 'budgetsubitem'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('MechanicItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('MechanicItem'),
        glyph: 0xf4fb,
        widget: 'mechanic'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('MechanicTypeItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('MechanicTypeItem'),
        glyph: 0xf4fc,
        widget: 'mechanictype'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('PromoStatusItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('PromoStatusItem'),
        glyph: 0xf0ef,
        widget: 'promostatus'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('RejectReasonItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('RejectReasonItem'),
        glyph: 0xf739,
        widget: 'rejectreason'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('ColorItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ColorItem'),
        glyph: 0xf266,
        widget: 'color'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('EventItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('EventItem'),
        glyph: 0xf0f6,
        widget: 'event'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('NodeTypeItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('NodeTypeItem'),
        glyph: 0xf645,
        widget: 'nodetype'
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('NoneNego'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('NoneNego'),
        glyph: 0xf13a,
        widget: 'nonenego'
    }]
    }, {
        text: l10n.ns('tpm', 'mainmenu').value('Finance'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('Finance'),
        scale: 'medium',
        glyph: 0xf81e,
        children: [{
            text: l10n.ns('tpm', 'mainmenu').value('COGS'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('COGS'),
            glyph: 0xf152,
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
            widget: 'cogs'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
            glyph: 0xfb2d,
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
            widget: 'tradeinvestment'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
            glyph: 0xfb2d,
            //roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
            widget: 'promoroireport'
        }]
    }]);