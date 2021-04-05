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
        { name: 'Technology_Description_ru', type: 'string', isDefault: true },
        { name: 'TechnologySubBrand', type: 'string', isDefault: true },
        { name: 'BrandTech_code', type: 'string', isDefault: true },
        { name: 'BrandsegTechsub_code', type: 'string', hidden: false, isDefault: true }
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
