Ext.define('App.model.tpm.promoroireport.PromoROIReport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Promo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'TechnologyId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreMechanicTypeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },

        { name: 'Number', type: 'int', hidden: false, isDefault: true },

        { name: 'Client1LevelName', type: 'string', hidden: false, isDefault: true },//NA/RKA (1 level)
        { name: 'Client2LevelName', type: 'string', hidden: false, isDefault: true },//Client Group (2 level)
        { name: 'ClientName', type: 'string', hidden: false, isDefault: true },//Client (3 level)

        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: false },
        { name: 'TechnologyName', mapping: 'Technology.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Technology', type: 'string', hidden: false, isDefault: true },//Product (2 level)
        { name: 'ProductSubrangesList', type: 'string', hidden: false, isDefault: true },//Product (3 level)

        { name: 'MarsMechanicName', type: 'string', mapping: 'MarsMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', mapping: 'MarsMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MarsMechanicType', useNull: true, hidden: true, isDefault: true },
        { name: 'MarsMechanicDiscount', type: 'int', hidden: false, isDefault: false },
        { name: 'MechanicComment', type: 'string', hidden: false, isDefault: false },

        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: false },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false },
        { name: 'PromoDuration', useNull: true, type: 'int', hidden: false, isDefault: false },

        { name: 'DispatchDuration', useNull: true, type: 'int', hidden: false, isDefault: false },

        { name: 'EventName', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },

        { name: 'PlanInstoreMechanicName', type: 'string', type: 'string', mapping: 'PlanInstoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PlanInstoreMechanic', useNull: true, hidden: true, isDefault: true },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', type: 'string', mapping: 'PlanInstoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PlanInstoreMechanicType', useNull: true, hidden: true, isDefault: true },
        { name: 'PlanInstoreMechanicDiscount', type: 'int', hidden: false, isDefault: false },

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
        { name: 'PlanPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'PlanPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },       
        { name: 'PlanPostPromoEffectW1', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPostPromoEffectW2', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPostPromoEffect', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'PlanPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoNetROIPercent', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoNetUpliftPercent', useNull: true, type: 'int', hidden: false, isDefault: false },

        { name: 'ActualInStoreMechanicName', type: 'string', mapping: 'ActualInStoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'ActualInStoreMechanic', useNull: true, hidden: true, isDefault: true },
        { name: 'ActualInStoreMechanicTypeName', type: 'string', mapping: 'ActualInStoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'ActualInStoreMechanicType', useNull: true, hidden: true, isDefault: true },
        { name: 'ActualInStoreMechanicDiscount', type: 'int', hidden: false, isDefault: false },
        { name: 'ActualInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
    
        { name: 'ActualPromoProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },

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
        { name: 'ActualPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'FactPostPromoEffectW1', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'FactPostPromoEffectW2', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'FactPostPromoEffect', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'ActualPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'ActualPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'ActualPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'ActualPromoNetROIPercent', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'ActualPromoNetUpliftPercent', useNull: true, type: 'int', hidden: false, isDefault: false },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Promoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});