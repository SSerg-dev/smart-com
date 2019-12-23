Ext.define('App.model.tpm.brand.Brand', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Brand',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Brand_code', type: 'string', hidden: false, isDefault: true },
        { name: 'Segmen_code', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Brands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
