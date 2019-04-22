Ext.define('App.model.tpm.subrange.DeletedSubrange', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Subrange',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedSubranges',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
