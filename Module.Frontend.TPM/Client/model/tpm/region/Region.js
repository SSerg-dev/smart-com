Ext.define('App.model.tpm.region.Region', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Region',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Regions',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
