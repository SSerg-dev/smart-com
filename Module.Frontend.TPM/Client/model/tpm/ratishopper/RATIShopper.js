Ext.define('App.model.tpm.ratishopper.RATIShopper', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RATIShopper',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Year', type: 'int', hidden: false, isDefault: true, },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true
        },
        { name: 'RATIShopperPercent', type: 'float', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RATIShoppers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});