﻿Ext.define('App.model.tpm.promoroireport.PromoROIReport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Promo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'Client1LevelName', type: 'string', hidden: false, isDefault: true },//NA/RKA (1 level)
        { name: 'Client2LevelName', type: 'string', hidden: false, isDefault: true },//Client Group (2 level)
        { name: 'ClientName', type: 'string', hidden: false, isDefault: true },//Client (3 level)
        { name: 'BrandName', type: 'string', hidden: false, isDefault: false },
        { name: 'TechnologyName', type: 'string', hidden: false, isDefault: true },
        { name: 'SubName', type: 'string', hidden: false, isDefault: true },//Product (2 level)
        { name: 'ProductSubrangesList', type: 'string', hidden: false, isDefault: true },//Product (3 level)
        { name: 'MarsMechanicName', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', hidden: true, isDefault: false },
        { name: 'MarsMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'MechanicComment', type: 'string', hidden: false, isDefault: false },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsStartDate', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsEndDate', type: 'string', useNull: true, hidden: false, isDefault: false },
        { name: 'BudgetYear', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PromoDuration', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'EventName', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoStatusName', type: 'string', hidden: false, isDefault: true },
        { name: 'InOut', useNull: true, type: 'bool', hidden: false, isDefault: true },
        { name: 'IsGrowthAcceleration', type: 'bool', hidden: false, isDefault: true },
        { name: 'PlanInstoreMechanicName', type: 'string', type: 'string', hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', type: 'string', hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'PlanInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        // [{ name: 'PcPrice' }] 19
        { name: 'PCPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBranding', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBTL', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        // [{ name: 'TI Base' }] 36
        { name: 'TIBasePercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        // [{ name: 'COGS' }] 39
        { name: 'COGSPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'COGSTn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },

        //Add TI
        { name: 'ActualAddTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualAddTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanAddTIMarketingApproved', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanAddTIShopperCalculated', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanAddTIShopperApproved', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'ActualInStoreMechanicName', type: 'string', hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicTypeName', type: 'string', hidden: true, isDefault: false },
        { name: 'ActualInStoreDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'ActualInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSVByCompensation', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSV', useNull: true, defaultValue: 0, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBranding', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBTL', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        //New calculation parameters for ROI
        { name: 'PlanPromoIncrementalMACLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalMACLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalMACLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalMACLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalEarningsLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalEarningsLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalEarningsLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalEarningsLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercentLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetROIPercentLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercentLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetROIPercentLSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'PromoTypesName', useNull: true, type: 'string', hidden: false, isDefault: false },
        { name: 'SumInvoice', useNull: true, type: 'float', hidden: false, isDefault: false },
        // add new prop
        { name: 'PlanPromoIncrementalCOGSTn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalCOGSTn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalCOGSTn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalCOGSTn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaselineVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: false },
        { name: 'IsApolloExport', type: 'bool', hidden: false, isDefault: true },
        { name: 'MLmodel', type: 'bool', hidden: false, isDefault: true },
        { name: 'PlanPromoVolume', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNSVtn', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNSVtn', useNull: true, type: 'float', hidden: false, isDefault: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoROIReports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: TpmModes.Prod.alias
        }
    },
});