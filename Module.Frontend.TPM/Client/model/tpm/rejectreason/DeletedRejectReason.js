Ext.define('App.model.tpm.rejectreason.DeletedRejectReason', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RejectReason',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'SystemName', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedRejectReasons',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
