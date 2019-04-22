Ext.define('App.model.tpm.variety.Variety', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Variety',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Varieties',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
