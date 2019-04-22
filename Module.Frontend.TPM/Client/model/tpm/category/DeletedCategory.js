Ext.define('App.model.tpm.category.DeletedCategory', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Category',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCategories',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
