Ext.define('App.model.tpm.competitorpromo.HistoricalCompetitorPromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'CompetitorPromo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'CompetitorName', type: 'string', isDefault: true},
        { name: 'ClientTreeFullPathName', type: 'string', isDefault: true },
        { name: 'CompetitorBrandTechName', type: 'string', isDefault: true},
        { name: 'Name', type: 'string', isDefault: true },
        { name: 'Number', type: 'int', isDefault: true },
        { name: 'StartDate', type: 'date', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'Discount', type: 'float', isDefault: true },
        { name: 'Price', type: 'float', isDefault: true },
        { name: 'Subrange', type: 'string', isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCompetitorPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            //Id промо для метода GetById в истории
            Id: null
        }
    }
});
