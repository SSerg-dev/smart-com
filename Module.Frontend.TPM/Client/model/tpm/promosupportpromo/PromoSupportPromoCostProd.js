﻿Ext.define('App.model.tpm.promosupportpromo.PromoSupportPromoCostProd', {
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
        { name: 'PlanCostProd', type: 'float', isDefault: true },
        { name: 'FactCostProd', type: 'float', isDefault: true },
        { name: 'EventName', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.EventName' },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.EndDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusName', type: 'string', mapping: 'Promo.PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        { name: 'BudgetSubItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.Name', hidden: true },
        { name: 'BudgetItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.BudgetItem.Name', hidden: true },

        { name: 'PlanCalculation', type: 'float', hidden: true },
        { name: 'FactCalculation', type: 'float', hidden: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoSupportPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
