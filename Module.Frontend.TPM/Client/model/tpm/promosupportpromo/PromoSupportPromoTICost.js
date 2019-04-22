Ext.define('App.model.tpm.promosupportpromo.PromoSupportPromoTICost', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupportPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'PromoSupportId', hidden: true },

        { name: 'Number', type: 'int', hidden: false, isDefault: true, mapping: 'Promo.Number' },
        { name: 'Name', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.Name' },
        { name: 'BrandTechName', type: 'string', mapping: 'Promo.BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'PlanCalculation', type: 'float', isDefault: true },
        { name: 'FactCalculation', type: 'float', isDefault: true },
        { name: 'EventName', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.EventName' },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.StartDate' },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.EndDate' },
        { name: 'PromoStatusName', type: 'string', mapping: 'Promo.PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        { name: 'BudgetSubItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.Name', hidden: true },
        { name: 'BudgetItemName', isDefault: false, mapping: 'PromoSupport.BudgetSubItem.BudgetItem.Name', hidden: true },       

        { name: 'PlanCostProd', type: 'float', hidden: true },
        { name: 'FactCostProd', type: 'float', hidden: true },
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
