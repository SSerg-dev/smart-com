Ext.define('App.model.tpm.pricelist.PriceList', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PriceList',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ProductId', hidden: true, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeGHierarchyCode', type: 'string', mapping: 'ClientTree.GHierarchyCode', defaultFilterConfig: { valueField: 'GHierarchyCode' }, hidden: false, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true, defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true },
        { name: 'ProductZREP', type: 'string', isDefault: true, hidden: false, mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' }, breezeEntityType: 'Product' },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'Price', type: 'float', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PriceLists',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
