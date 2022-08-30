Ext.define('App.model.tpm.incrementalpromo.IncrementalPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'IncrementalPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
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
            hidden: false, isDefault: true, isKey: true
        },
        { name: 'PromoName', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.Name' },  
        { name: 'PlanPromoIncrementalCases', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoIncrementalLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'CasePrice', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PromoDispatchStartDate', type: 'date', hidden: true, mapping: 'Promo.DispatchesStart', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsGrowthAcceleration' },
        { name: 'IsInExchange', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsInExchange' },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'IncrementalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: 'Current'
        }
    }
});