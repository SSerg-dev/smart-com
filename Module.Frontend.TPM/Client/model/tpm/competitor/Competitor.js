Ext.define('App.model.tpm.competitor.Competitor', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Competitor',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Competitors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
