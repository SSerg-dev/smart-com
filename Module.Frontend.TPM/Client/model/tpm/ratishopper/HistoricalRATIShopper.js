Ext.define('App.model.tpm.ratishopper.HistoricalRATIShopper', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'RATIShopper',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'Year', type: 'int', hidden: false, isDefault: true },
        { name: 'RATIShopperPercent', type: 'float', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalRATIShoppers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            Id: null
        }
    }
});
