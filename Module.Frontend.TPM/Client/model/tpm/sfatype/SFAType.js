Ext.define('App.model.tpm.sfatype.SFAType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'SFAType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'SFATypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
