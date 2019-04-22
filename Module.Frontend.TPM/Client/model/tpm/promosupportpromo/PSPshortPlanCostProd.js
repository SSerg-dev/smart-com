Ext.define('App.model.tpm.promosupportpromo.PSPshortPlanCostProd', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupportPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'PromoSupportId', hidden: true },

        { name: 'BudgetSubItemName', type: 'string', mapping: 'PromoSupport.BudgetSubItem.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BudgetSubItem', isDefault: true },
        { name: 'PlanCostProd', type: 'float', isDefault: true },
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
