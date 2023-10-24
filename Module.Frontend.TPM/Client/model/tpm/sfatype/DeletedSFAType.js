Ext.define('App.model.tpm.sfatype.DeletedSFAType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'SFAType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedSFATypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
