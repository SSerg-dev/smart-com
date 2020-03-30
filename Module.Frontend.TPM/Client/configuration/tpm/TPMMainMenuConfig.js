MenuMgr.defineMenu([
    {
        text: l10n.ns('tpm', 'mainmenu').value('UserDashboard'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('UserDashboard'),
        scale: 'medium',
        glyph: 0xfa1c,
        roles: ['DemandFinance', 'KeyAccountManager', 'DemandPlanning', 'CustomerMarketing', 'CMManager'],
        widget: 'userdashboard'
    },
    {
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
        text: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
        scale: 'medium',
        glyph: 0xf1b3,
        children: [{
            text: l10n.ns('tpm', 'mainmenu').value('TICosts'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
            glyph: 0xf1b3,
            //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'SuperReader'],
            widget: 'associatedpromosupport',
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('NonPromoTICosts'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('NonPromoSupport'),
            glyph: 0xf1b3,
            //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'SuperReader'],
            widget: 'associatednonpromosupport',
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
            glyph: 0xf1b3,
            //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'SuperReader'],
            widget: 'associatedcostproduction'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('BTL'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('BTL'),
            glyph: 0xf1b3,
            //roles: [],
            widget: 'associatedbtlpromo'
        }]
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
            text: l10n.ns('tpm', 'mainmenu').value('ClientTreeBrandTech'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('ClientTreeBrandTech'),
            glyph: 0xf007,
            widget: 'clienttreebrandtech'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('AssortmentMatrix'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('AssortmentMatrix'),
            glyph: 0xf007,
            widget: 'assortmentmatrix'
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
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('PromoProductCorection'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PromoProductCorection'),
            glyph: 0xf619,

            widget: 'promoproductcorrection'
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
            roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning'],
            widget: 'actual'
        }, */{
                text: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
                glyph: 0xf127,
                scale: 'medium',
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
                widget: 'baseline'
            }, {
                text: l10n.ns('tpm', 'mainmenu').value('DemandPriceListItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('DemandPriceListItem'),
                glyph: 0xf127,
                scale: 'medium',
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
                widget: 'demandpricelist'
            }, {
                text: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
                glyph: 0xf127,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'planincrementalreport'
            }, {
                text: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectReport'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectReport'),
                glyph: 0xf127,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'planpostpromoeffectreport'
            }, {
                text: l10n.ns('tpm', 'mainmenu').value('IncrementalPromo'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('IncrementalPromo'),
                glyph: 0xf127,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'incrementalpromo'
            }, {
                text: l10n.ns('tpm', 'mainmenu').value('ActualLSV'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ActualLSV'),
                glyph: 0xf127,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'actuallsv'
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
            text: l10n.ns('tpm', 'mainmenu').value('PromoTypes'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
            glyph: 0xf218,
            widget: 'promotypes'
        },
        {
            text: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
            glyph: 0xf0be,
            widget: 'budgetitem'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
            glyph: 0xf0be,
            widget: 'associatedbudgetsubitemclienttree'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('NonPromoEquipment'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('NonPromoEquipment'),
            glyph: 0xf0be,
            widget: 'nonpromoequipment'
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
            widget: 'associatedeventclienttree'
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
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
            widget: 'cogs'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('ActualCOGS'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('ActualCOGS'),
            glyph: 0xf152,
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
            widget: 'actualcogs'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
            glyph: 0xfb2d,
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
            widget: 'tradeinvestment'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('ActualTradeInvestment'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('ActualTradeInvestment'),
            glyph: 0xfb2d,
            roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
            widget: 'actualtradeinvestment'
        }, {
            text: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
            tooltip: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
            glyph: 0xf215,
            //roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
            widget: 'promoroireport'
        }]
    },
    //{
    //    text: l10n.ns('tpm', 'mainmenu').value('ClientDashboard'),
    //    tooltip: l10n.ns('tpm', 'mainmenu').value('ClientDashboard'),
    //    scale: 'medium',
    //    glyph: 0xF56E,
    //    children: [{
    //        text: l10n.ns('tpm', 'mainmenu').value('Dashboard'),
    //        tooltip: l10n.ns('tpm', 'mainmenu').value('Dashboard'),
    //        glyph: 0xFA1C,
    //        widget: 'clientdashboard'
    //    }, {
    //        text: l10n.ns('tpm', 'mainmenu').value('ClientKPIdata'),
    //        tooltip: l10n.ns('tpm', 'mainmenu').value('ClientKPIdata'),
    //        glyph: 0xF572,
    //        widget: 'clientkpidata'
    //    }]
    //}
]);