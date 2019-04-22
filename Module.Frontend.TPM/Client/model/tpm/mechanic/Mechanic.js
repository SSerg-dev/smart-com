Ext.define('App.model.tpm.mechanic.Mechanic', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Mechanic',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'SystemName', useNull: true, type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Mechanics',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
