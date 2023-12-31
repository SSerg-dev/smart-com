﻿Ext.define('App.model.tpm.promosupportpromo.PromoSupportPromoTICost', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupportPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'PromoSupportId', hidden: true },

        { name: 'Number', type: 'int', hidden: false, isDefault: true, mapping: 'Promo.Number', isKey: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.Name' },
        { name: 'BrandTechName', type: 'string', mapping: 'Promo.BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'PlanCalculation', type: 'float', isDefault: true },
        { name: 'FactCalculation', type: 'float', isDefault: true },
        { name: 'EventName', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.EventName' },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.EndDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusName', type: 'string', mapping: 'Promo.PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        { name: 'BudgetSubItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.Name', hidden: true },
        { name: 'BudgetItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.BudgetItem.Name', hidden: true },       

        { name: 'PlanCostProd', type: 'float', hidden: true },
        { name: 'FactCostProd', type: 'float', hidden: true },

        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoDispatchStartDate', type: 'date', hidden: true, mapping: 'Promo.DispatchesStart', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsGrowthAcceleration' },
        { name: 'IsInExchange', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsInExchange' },
        { name: 'PromoBudgetYear', type: 'int', mapping: 'Promo.BudgetYear', hidden: true, isDefault: false },
        
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoSupportPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: TpmModes.Prod.alias
        }
    }
});
