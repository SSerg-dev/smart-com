Ext.define('App.model.tpm.rejectreason.RejectReason', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RejectReason',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'SystemName', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RejectReasons',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
