Ext.define('App.model.tpm.competitorbrandtech.CompetitorBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CompetitorBrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Color', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'CompetitorId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompetitorName', type: 'string', mapping: 'Competitor.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Competitor', hidden: false, isDefault: true
        },
        { name: 'BrandTech', useNull: false, type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CompetitorBrandTechs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
