Ext.define('App.model.tpm.coefficientsi2so.HistoricalCoefficientSI2SO', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'CoefficientSI2SO',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'CoefficientValue', type: 'float', hidden: false, isDefault: true },
        { name: 'Lock', type: 'boolean', hidden: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', isDefault: true },
        { name: 'BrandTechBrandTech_code', type: 'string', isDefault: true } 
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCoefficientSI2SOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
