Ext.define('App.model.tpm.incrementalpromo.IncrementalPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'IncrementalPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true, isDefault: false },
        { name: 'ProductId', hidden: true, isDefault: false },
        {
            name: 'ProductZREP', type: 'string', mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        {
            name: 'ProductName', type: 'string', mapping: 'Product.ProductEN', defaultFilterConfig: { valueField: 'ProductEN' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        {
            name: 'PromoClient', type: 'string', mapping: 'Promo.ClientHierarchy', defaultFilterConfig: { valueField: 'FullPathName' },
            breezeEntityType: 'ClientTree', hidden: false, isDefault: true, tree: true, viewTree: true
        },
        {
            name: 'PromoNumber', type: 'int', mapping: 'Promo.Number', defaultFilterConfig: { valueField: 'Number' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true, isKey: true
        },
        {
            name: 'PromoName', type: 'string', mapping: 'Promo.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },            
        { name: 'PlanPromoIncrementalCases', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'CasePrice', type: 'float', hidden: false, isDefault: true, useNull: true  },
        { name: 'PlanPromoIncrementalLSV', type: 'float', hidden: false, isDefault: true, useNull: true  },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'IncrementalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});