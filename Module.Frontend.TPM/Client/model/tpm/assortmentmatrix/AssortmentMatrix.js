Ext.define('App.model.tpm.assortmentmatrix.AssortmentMatrix', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AssortmentMatrix',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeName', type: 'string', mapping: 'ClientTree.FullPathName',
            tree: true, defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        { name: 'ProductId', hidden: true, isDefault: true },
        {
            name: 'EAN_PC', type: 'string', mapping: 'Product.EAN_PC', tree: true,
            defaultFilterConfig: { valueField: 'EAN_PC' }, breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'AssortmentMatrices',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
    }
});
