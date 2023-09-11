MenuMgr.defineMenu([
    {
        text: l10n.ns('tpm', 'mainmenu').value('ClientDashboard'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ClientDashboard'),
        scale: 'medium',
        glyph: 0xF56E,
        rsMode: true,
        raMode: true,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('Dashboard'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('Dashboard'),
                glyph: 0xFA1C,
                rsMode: true,
                raMode: true,
                currentMode: true,
                widget: 'clientdashboard'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ClientKPIdata'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ClientKPIdata'),
                glyph: 0xF572,
                rsMode: true,
                raMode: true,
                currentMode: true,
                widget: 'clientkpidata'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ClientKPIdataRS'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ClientKPIdataRS'),
                glyph: 0xF572,
                rsMode: true,
                raMode: true,
                currentMode: false,
                widget: 'clientkpidatars'
            },
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('UserDashboard'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('UserDashboard'),
        scale: 'medium',
        glyph: 0xfa1c,
        rsMode: true,
        raMode: true,
        currentMode: true,
        roles: ['DemandFinance', 'KeyAccountManager', 'DemandPlanning', 'CustomerMarketing', 'CMManager', 'GAManager'],
        widget: 'userdashboard'
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('BusinessMetrics'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('BusinessMetrics'),
        scale: 'medium',
        glyph: 0xF873,
        rsMode: false,
        raMode: false,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('MetricsDashboard'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('MetricsDashboard'),
                scale: 'medium',
                glyph: 0xF873,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'metricsdashboard'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('MetricsLiveHistory'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('MetricsLiveHistory'),
                scale: 'medium',
                glyph: 0xF2DA,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'SupportAdministrator'],
                widget: 'metricslivehistory'
            }
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('SchedulerItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('SchedulerItem'),
        glyph: 0xf0f1,
        rsMode: true,
        raMode: true,
        currentMode: true,
        scale: 'medium',
        widget: 'schedulecontainer'
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('PromoItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('PromoItem'),
        scale: 'medium',
        rsMode: true,
        raMode: true,
        currentMode: true,
        glyph: 0xf392,
        widget: 'promo'
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
        scale: 'medium',
        glyph: 0xf1b3,
        rsMode: true,
        raMode: true,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('TICosts'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoSupport'),
                glyph: 0xf1b3,
                rsMode: true,
                raMode: true,
                currentMode: true,
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'SuperReader'],
                widget: 'associatedpromosupport',
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('NonPromoTICosts'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('NonPromoSupport'),
                glyph: 0xf1b3,
                rsMode: false,
                raMode: false,
                currentMode: true,
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'SuperReader'],
                widget: 'associatednonpromosupport',
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('CostProduction'),
                glyph: 0xf1b3,
                rsMode: false,
                raMode: false,
                currentMode: true,
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'SuperReader'],
                widget: 'associatedcostproduction'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BTL'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BTL'),
                glyph: 0xf1b3,
                rsMode: false,
                raMode: false,
                currentMode: true,
                //roles: [],
                widget: 'associatedbtlpromo'
            }
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('CompetitorPromo'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('CompetitorPromo'),
        scale: 'medium',
        glyph: 0xfd01,
        rsMode: false,
        raMode: false,
        currentMode: true,
        roles: ['Administrator', 'SupportAdministrator', 'CMManager'],
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('Competitor'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('Competitor'),
                glyph: 0xf14c,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'SupportAdministrator', 'CMManager'],
                widget: 'competitor',
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('CompetitorBrandTech'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('CompetitorBrandTech'),
                glyph: 0xf619,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'SupportAdministrator', 'CMManager'],
                widget: 'competitorbrandtech',
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('CompetitorPromo'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('CompetitorPromo'),
                glyph: 0xfd01,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'SupportAdministrator', 'CMManager'],
                widget: 'competitorpromo',
            }
        ],
    },
    {
        text: l10n.ns('core', 'mainmenu').value('RPAItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('RPAItem'),
        scale: 'medium',
        glyph: 0xf494,
        rsMode: false,
        raMode: false,
        currentMode: true,
        roles: [UserRoles.Administrator,
            UserRoles.SupportAdministrator,
            UserRoles.KeyAccountManager,
            UserRoles.DemandPlanning,
            UserRoles.DemandFinance,
            UserRoles.CustomerMarketingManager,
            UserRoles.CustomerMarketing,
            UserRoles.FunctionalExpert
        ],
        widget: 'rpa'
    },
    {
        text: l10n.ns('core', 'mainmenu').value('RSmode'),
        tooltip: l10n.ns('core', 'mainmenu').value('RSmode'),
        scale: 'medium',
        glyph: 0xf149,
        rsMode: true,
        raMode: true,
        currentMode: false,
        roles: [UserRoles.Administrator,
            UserRoles.SupportAdministrator,
            UserRoles.KeyAccountManager,
            UserRoles.DemandPlanning,
            UserRoles.DemandFinance,
            UserRoles.CMManager,
            UserRoles.CustomerMarketing,
            UserRoles.CustomerMarketingManager,
            UserRoles.FunctionalExpert,
            UserRoles.GrowthAccelerationManager,
            UserRoles.SuperReader
        ],
        widget: 'rsmode'
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('ClientItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ClientItem'),
        scale: 'medium',
        glyph: 0xf007,
        rsMode: false,
        raMode: false,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('ClientTreeItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ClientTreeItem'),
                glyph: 0xf007,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'clienttree'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BaseClientTreeView'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BaseClientTreeView'),
                glyph: 0xf007,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'baseclienttreeview'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('RetailType'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('RetailType'),
                glyph: 0xf110,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'retailtype'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ClientTreeBrandTech'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ClientTreeBrandTech'),
                glyph: 0xf007,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'clienttreebrandtech'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('AssortmentMatrix'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('AssortmentMatrix'),
                glyph: 0xf007,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'assortmentmatrix'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PLUDictionary'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PLUDictionary'),
                glyph: 0xf007,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'pludictionary',
                roles: ['Administrator', 'KeyAccountManager', 'DemandPlanning', 'SupportAdministrator']
            }
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
        scale: 'medium',
        glyph: 0xf1a6,
        rsMode: true,
        raMode: true,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ProductItem'),
                glyph: 0xf1a6,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'product'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ProductTreeItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ProductTreeItem'),
                glyph: 0xf645,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'producttree'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BrandItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BrandItem'),
                glyph: 0xf071,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'brand'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('TechnologyItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('TechnologyItem'),
                glyph: 0xf3d3,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'technology'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BrandTechItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BrandTechItem'),
                glyph: 0xf619,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'brandtech'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoProductCorection'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoProductCorection'),
                glyph: 0xf619,
                rsMode: true,
                raMode: true,
                currentMode: true,
                widget: 'promoproductcorrection'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoProductCorrectionPriceIncrease'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoProductCorrectionPriceIncrease'),
                glyph: 0xf619,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'promoproductcorrectionpriceincrease'
            },
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('DemandItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('DemandItem'),
        scale: 'medium',
        glyph: 0xf127,
        rsMode: true,
        raMode: true,
        currentMode: true,
        children: [/*{
			text: l10n.ns('tpm', 'mainmenu').value('ActualItem'),
			tooltip: l10n.ns('tpm', 'mainmenu').value('ActualItem'),
			glyph: 0xf127,
			scale: 'medium',
			roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning'],
			widget: 'actual'
		}, */
            {
                text: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BaseLineItem'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
                widget: 'baseline'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('IncreaseBaseLineItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('IncreaseBaseLineItem'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
                widget: 'increasebaseline'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PlanPostPromoEffectItem'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                //roles: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'DemandFinance', 'DemandPlanning', 'KeyAccountManager'],
                widget: 'planpostpromoeffect'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PlanIncrementalReport'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'planincrementalreport'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('IncrementalPromo'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('IncrementalPromo'),
                glyph: 0xf127,
                rsMode: true,
                raMode: true,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'incrementalpromo'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ActualLSV'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ActualLSV'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'actuallsv'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PriceList'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PriceList'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'FunctionalExpert', 'DemandPlanning', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'pricelist'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('CoefficientSI2SO'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('CoefficientSI2SO'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'DemandPlanning', 'SuperReader', 'SupportAdministrator'],
                widget: 'coefficientsi2so'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PreviousDayIncremental'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PreviousDayIncremental'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'DemandPlanning', 'SuperReader', 'SupportAdministrator'],
                widget: 'previousdayincremental'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('RollingVolume'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PreviousDayIncremental'),
                glyph: 0xf127,
                rsMode: false,
                raMode: false,
                currentMode: true,
                scale: 'medium',
                roles: ['Administrator', 'DemandPlanning', 'SupportAdministrator', 'SuperReader'],
                widget: 'rollingvolume'
            }
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('TablesItem'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('TablesItem'),
        scale: 'medium',
        glyph: 0xf0ba,
        rsMode: false,
        raMode: false,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
                glyph: 0xf218,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'budget'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoTypes'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItem'),
                glyph: 0xf218,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'promotypes'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetItemItem'),
                glyph: 0xf0be,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'budgetitem'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('BudgetSubItemItem'),
                glyph: 0xf0be,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'associatedbudgetsubitemclienttree'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('NonPromoEquipment'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('NonPromoEquipment'),
                glyph: 0xf0be,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'nonpromoequipment'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('MechanicItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('MechanicItem'),
                glyph: 0xf4fb,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'mechanic'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('MechanicTypeItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('MechanicTypeItem'),
                glyph: 0xf4fc,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'mechanictype'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoStatusItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoStatusItem'),
                glyph: 0xf0ef,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'promostatus'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('RejectReasonItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('RejectReasonItem'),
                glyph: 0xf739,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'rejectreason'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ColorItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ColorItem'),
                glyph: 0xf266,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'color'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('EventItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('EventItem'),
                glyph: 0xf0f6,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'associatedeventclienttree'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('NodeTypeItem'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('NodeTypeItem'),
                glyph: 0xf645,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'nodetype'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('NoneNego'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('NoneNego'),
                glyph: 0xf13a,
                rsMode: false,
                raMode: false,
                currentMode: true,
                widget: 'nonenego'
            }
        ]
    },
    {
        text: l10n.ns('tpm', 'mainmenu').value('Finance'),
        tooltip: l10n.ns('tpm', 'mainmenu').value('Finance'),
        scale: 'medium',
        glyph: 0xf81e,
        rsMode: true,
        raMode: true,
        currentMode: true,
        children: [
            {
                text: l10n.ns('tpm', 'mainmenu').value('COGS'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('COGS'),
                glyph: 0xf152,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'cogs'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ActualCOGS'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ActualCOGS'),
                glyph: 0xf152,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'actualcogs'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PlanCOGSTn'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PlanCOGSTn'),
                glyph: 0xf152,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'plancogstn'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ActualCOGSTn'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ActualCOGSTn'),
                glyph: 0xf152,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'actualcogstn'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('TradeInvestment'),
                glyph: 0xfb2d,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'tradeinvestment'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('ActualTradeInvestment'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('ActualTradeInvestment'),
                glyph: 0xfb2d,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'SupportAdministrator'],
                widget: 'actualtradeinvestment'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoROIReport'),
                glyph: 0xf215,
                rsMode: true,
                raMode: true,
                currentMode: true,
                //roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
                widget: 'promoroireport'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('PromoPriceIncreaseROIReport'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('PromoPriceIncreaseROIReport'),
                glyph: 0xf215,
                rsMode: true,
                raMode: true,
                currentMode: true,
                //roles: ['Administrator', 'FunctionalExpert', 'DemandFinance', 'SuperReader'],
                widget: 'promopriceincreaseroireport'
            },
            {
                text: l10n.ns('tpm', 'mainmenu').value('RATIShopper'),
                tooltip: l10n.ns('tpm', 'mainmenu').value('RATIShopper'),
                glyph: 0xfcce,
                rsMode: false,
                raMode: false,
                currentMode: true,
                roles: ['Administrator', 'DemandFinance', 'SupportAdministrator', 'SuperReader', 'CustomerMarketing', 'CMManager', 'FunctionalExpert'],
                widget: 'ratishopper'
            }
        ]
    }
]);