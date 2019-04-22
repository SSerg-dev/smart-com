Ext.define('App.model.tpm.techhighlevel.DeletedTechHighLevel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'TechHighLevel',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedTechHighLevels',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
