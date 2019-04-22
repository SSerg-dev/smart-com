Ext.define('App.model.tpm.agegroup.DeletedAgeGroup', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AgeGroup',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedAgeGroups',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
