Ext.define('App.model.tpm.increasebaseline.IncreaseBaseLine', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'IncreaseBaseLine',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ProductId', hidden: true, isDefault: false },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true }, 
        { name: 'InputBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'SellInBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'SellOutBaselineQTY', type: 'float', hidden: false, isDefault: true },
        { name: 'Type', type: 'int', hidden: false, isDefault: true },
        {
            name: 'ProductZREP', type: 'string', mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'IncreaseBaseLines',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});