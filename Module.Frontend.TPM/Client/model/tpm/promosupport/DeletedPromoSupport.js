Ext.define('App.model.tpm.region.DeletedRegion', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Region',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedRegions',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
