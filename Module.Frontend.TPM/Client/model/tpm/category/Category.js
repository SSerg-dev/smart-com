Ext.define('App.model.tpm.category.Category', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Category',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Categories',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
