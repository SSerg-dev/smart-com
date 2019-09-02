Ext.define('App.model.tpm.brandtech.HistoricalBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'BrandTech',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'BrandId', hidden: true, isDefault: true },
        { name: 'TechnologyId', hidden: true, isDefault: true },
        { name: 'BrandName', type: 'string', isDefault: true },
        { name: 'TechnologyName', type: 'string', isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalBrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
