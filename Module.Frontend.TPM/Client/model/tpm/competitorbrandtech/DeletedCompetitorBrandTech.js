Ext.define('App.model.tpm.competitorbrandtech.DeletedCompetitorBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CompetitorBrandTech',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Color', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'CompetitorId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompetitorName', type: 'string', mapping: 'Competitor.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Brand', hidden: false, isDefault: true
        },
        { name: 'BrandTech', useNull: false, type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCompetitorBrandTechs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
