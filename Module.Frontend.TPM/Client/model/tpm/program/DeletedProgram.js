Ext.define('App.model.tpm.program.DeletedProgram', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Program',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPrograms',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
