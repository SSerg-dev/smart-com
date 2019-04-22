Ext.define('App.model.tpm.variety.DeletedVariety', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Variety',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedVarieties',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
