Ext.define('App.model.tpm.assortmentmatrix.AssortmentMatrix', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AssortmentMatrix',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeName', type: 'string', mapping: 'ClientTree.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'ClientTree', tree: true, hidden: false, isDefault: true
        },
        {
            name: 'PluCode', type: 'string', mapping: 'Plu.PluCode',
            defaultFilterConfig: { valueField: 'PluCode' }, breezeEntityType: 'Plu', hidden: false, isDefault: true
        },
        { name: 'ProductId', hidden: true, isDefault: true },
        {
            name: 'ProductEAN_PC', type: 'string', mapping: 'Product.EAN_PC', 
            defaultFilterConfig: { valueField: 'EAN_PC' }, breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true,            
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
        extraParams: {
            needActualAssortmentMatrix: false
        }
    }
});
