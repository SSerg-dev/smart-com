﻿Ext.define('App.model.tpm.clientkpidata.HistoricalClientKPIData', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'ClientDashboardView',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },

        { name: 'ObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'ClientHierarchy', type: 'string', mapping: 'ClientHierarchy', tree: true, viewTree: true, defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true },
        { name: 'ClientName', type: 'string', hidden: true, isDefault: false },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTechName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'LogoFileName', type: 'string', hidden: true, isDefault: false },
        { name: 'Year', type: 'int', hidden: false, isDefault: true },

        { name: 'ShopperTiPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ShopperTiPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'ShopperTiYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'ShopperTiYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ShopperTiYEE', type: 'float', hidden: false, isDefault: true },
        { name: 'ShopperTiYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'MarketingTiPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'MarketingTiPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'MarketingTiYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'MarketingTiYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'MarketingTiYEE', type: 'float', hidden: false, isDefault: true },
        { name: 'MarketingTiYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'ProductionPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ProductionPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'ProductionYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'ProductionYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ProductionYEE', type: 'float', hidden: false, isDefault: true },
        { name: 'ProductionYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'BrandingPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'BrandingPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'BrandingYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'BrandingYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'BrandingYEE', type: 'float', hidden: false, isDefault: true },
        { name: 'BrandingYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'BTLPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'BTLPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'BTLYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'BTLYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'BTLYEE', type: 'float', hidden: false, isDefault: true },
        { name: 'BTLYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'ROIPlanPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ROIYTDPercent', type: 'float', hidden: false, isDefault: true },
        { name: 'ROIYEEPercent', type: 'float', hidden: false, isDefault: true },

        { name: 'LSVPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'LSVYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'LSVYEE', type: 'float', hidden: false, isDefault: true },

        { name: 'IncrementalNSVPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'IncrementalNSVYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'IncrementalNSVYEE', type: 'float', hidden: false, isDefault: true },

        { name: 'PromoNSVPlan', type: 'float', hidden: false, isDefault: true },
        { name: 'PromoNSVYTD', type: 'float', hidden: false, isDefault: true },
        { name: 'PromoNSVYEE', type: 'float', hidden: false, isDefault: true },

        { name: 'PromoWeeks', type: 'int', hidden: true, isDefault: false },
        { name: 'VodYTD', type: 'float', hidden: true, isDefault: false },
        { name: 'VodYEE', type: 'float', hidden: true, isDefault: false },
        { name: 'ActualPromoLSV', type: 'float', hidden: true, isDefault: false },
        { name: 'PlanPromoLSV', type: 'float', hidden: true, isDefault: false }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalClientDashboards',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
