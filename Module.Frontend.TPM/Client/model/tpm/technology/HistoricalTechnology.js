Ext.define('App.model.tpm.technology.HistoricalTechnology', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Technology',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', type: 'string', hidden: false, isDefault: true },
        { name: 'Tech_code', type: 'string', hidden: false, isDefault: true },
        { name: 'SubBrand', type: 'string', hidden: false, isDefault: true },
        { name: 'SubBrand_code', type: 'string', hidden: false, isDefault: true },
        { name: 'IsSplittable', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalTechnologies',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
