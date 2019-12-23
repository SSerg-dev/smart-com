Ext.define('App.model.tpm.brand.DeletedBrand', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Brand',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Brand_code', type: 'string', hidden: false, isDefault: true },
        { name: 'Segmen_code', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBrands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
