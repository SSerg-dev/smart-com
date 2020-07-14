Ext.define('App.model.tpm.mechanictype.MechanicType', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'MechanicType',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Discount', type: 'float', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: false },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'MechanicTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
