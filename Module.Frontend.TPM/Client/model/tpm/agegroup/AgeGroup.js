Ext.define('App.model.tpm.agegroup.AgeGroup', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AgeGroup',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'AgeGroups',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
