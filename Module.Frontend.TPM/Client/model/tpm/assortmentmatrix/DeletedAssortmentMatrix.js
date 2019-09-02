Ext.define('App.model.tpm.assortmentmatrix.DeletedAssortmentMatrix', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AssortmentMatrix',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
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
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedAssortmentMatrices',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
