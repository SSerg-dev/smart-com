Ext.define('App.model.tpm.competitorbrandtech.HistoricalCompetitorBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'CompetitorBrandTech',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Color', type: 'string', isDefault: true },
        { name: 'CompetitorName', type: 'string', isDefault: true },
        { name: 'BrandTech', type: 'string', isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCompetitorBrandTechs',
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
