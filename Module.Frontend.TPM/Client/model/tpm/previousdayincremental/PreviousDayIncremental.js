Ext.define('App.model.tpm.previousdayincremental.PreviousDayIncremental', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PreviousDayIncremental',
    fields: [
        { name: 'Id', hidden: true },

        { name: 'PromoId', hidden: true },
        { name: 'ProductId', hidden: true }, 
        { name: 'Week', type: 'string', hidden: false, isDefault: true, useNull: true },    
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true, useNull: true },    
        { name: 'DMDGroup', type: 'string', hidden: true, isDefault: false, useNull: true },
        { name: 'IncrementalQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'LastChangeDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        {
            name: 'ZREP', type: 'string', mapping: 'Product.ZREP', breezeEntityType: 'Product', hidden: false, isDefault: true, defaultFilterConfig: { valueField: 'ZREP' }
        },
        {
            name: 'Number', type: 'int', mapping: 'Promo.Number', breezeEntityType: 'Promo', hidden: false, isDefault: true, defaultFilterConfig: { valueField: 'Number' },
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PreviousDayIncrementals',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
