Ext.define('App.model.tpm.promo.PromoGridView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoGridView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'ClientHierarchy', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTechName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'PromoEventName', type: 'string', mapping: 'PromoEventName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Event', hidden: false, isDefault: true },
        { name: 'Mechanic', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MechanicIA', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsStartDate', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsDispatchesStart', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatusName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
       
        { name: 'MarsMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'IsCMManagerApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'IsDemandPlanningApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'IsDemandFinanceApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicDiscount', type: 'float', hidden: false, isDefault: false },

        { name: 'StartDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DateStart', useNull: true, type: 'date', hidden: false, isDefault: false, mapping: 'StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },

        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },

        { name: 'LastChangedDate', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'LastChangedDateDemand', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone},
        { name: 'LastChangedDateFinance', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone},

        { name: 'MarsEndDate', type: 'string', useNull: true, hidden: false, isDefault: false },     
        { name: 'MarsDispatchesEnd', type: 'string', useNull: true, hidden: true, isDefault: false },

        { name: 'BrandName', type: 'string', mapping: 'BrandName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: false },
        
        { name: 'PromoStatusColor', type: 'string', mapping: 'PromoStatusColor', defaultFilterConfig: { valueField: 'Color' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'MarsMechanicName', type: 'string', mapping: 'MarsMechanicName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', mapping: 'MarsMechanicTypeName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },
        { name: 'PlanInstoreMechanicName', type: 'string', mapping: 'PlanInstoreMechanicName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatusSystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', mapping: 'PlanInstoreMechanicTypeName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },

        { name: 'PlanPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ProductHierarchy', type: 'string', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'CreatorId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },

        { name: 'InOut', type: 'boolean', hidden: false, isDefault: true },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'PlanPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: true },

        // Growth Acceleration
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoGridViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            // параметр указывающий на то, нужно ли фильтровать записи по признаку возможности перевода в другой статус
            canChangeStateOnly: false
        }
    }
});
