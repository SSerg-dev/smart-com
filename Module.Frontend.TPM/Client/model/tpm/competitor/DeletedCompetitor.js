Ext.define('App.model.tpm.competitor.DeletedCompetitor', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Competitor',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCompetitors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
