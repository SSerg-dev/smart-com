Ext.define('App.model.tpm.subrange.Subrange', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Subrange',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Subranges',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
