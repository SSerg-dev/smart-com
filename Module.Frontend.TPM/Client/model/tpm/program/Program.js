Ext.define('App.model.tpm.program.Program', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Program',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Programs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
