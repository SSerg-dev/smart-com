Ext.define('App.model.tpm.mechanictype.DeletedMechanicType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'MechanicType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Discount', useNull: true, type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedMechanicTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
