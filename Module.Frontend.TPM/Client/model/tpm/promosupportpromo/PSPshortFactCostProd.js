Ext.define('App.model.tpm.promosupportpromo.PSPshortFactCostProd', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSupportPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'PromoSupportId', hidden: true },

        { name: 'BudgetSubItemName', type: 'string', mapping: 'PromoSupport.BudgetSubItem.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BudgetSubItem', isDefault: true },
        { name: 'FactCostProd', type: 'float', isDefault: true },
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
