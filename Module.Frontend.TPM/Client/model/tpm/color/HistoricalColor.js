Ext.define('App.model.tpm.color.HistoricalColor', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Color',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'SystemName', type: 'string', isDefault: true },
        { name: 'DisplayName', type: 'string', isDefault: true },
        { name: 'BrandTechBrandName', type: 'string', isDefault: true },
        { name: 'BrandTechTechnologyName', type: 'string', isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalColors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
