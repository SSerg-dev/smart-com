Ext.define('App.model.tpm.technology.DeletedTechnology', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Technology',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Tech_code', type: 'string', hidden: false, isDefault: true },
        { name: 'SubBrand', type: 'string', hidden: false, isDefault: true },
        { name: 'SubBrand_code', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedTechnologies',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
