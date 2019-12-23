Ext.define('App.model.tpm.technology.Technology', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Technology',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Tech_code', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Technologies',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
