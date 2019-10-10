Ext.define('App.model.tpm.baseline.DeletedBaseLine', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BaseLine',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ProductId', hidden: true, isDefault: true },
        { name: 'DeletedDate', type: 'date', isDefault: false },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'QTY', type: 'float', hidden: false, isDefault: true },
        { name: 'Price', type: 'float', hidden: false, isDefault: true },
        { name: 'BaselineLSV', type: 'float', hidden: false, isDefault: true },
        { name: 'Type', type: 'int', hidden: false, isDefault: true },
        {
            name: 'ProductZREP', type: 'string', mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBaseLines',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});