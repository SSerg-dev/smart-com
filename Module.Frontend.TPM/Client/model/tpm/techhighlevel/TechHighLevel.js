Ext.define('App.model.tpm.techhighlevel.TechHighLevel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'TechHighLevel',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'TechHighLevels',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
