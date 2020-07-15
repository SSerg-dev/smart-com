Ext.define('App.model.tpm.nonenego.NoneNego', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'NoneNego',

    fields: [
        { name: 'Id', hidden: true },
        { name: 'MechanicId', hidden: true, isDefault: true },
        { name: 'MechanicTypeId', hidden: true, isDefault: true, useNull: true, defaultValue: null },
        { name: 'MechanicName', type: 'string', mapping: 'Mechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', hidden: false, isDefault: true },
        { name: 'MechanicTypeName', type: 'string', mapping: 'MechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', hidden: false, isDefault: true },
        { name: 'Discount', type: 'float', hidden: false, isDefault: true },
        { name: 'FromDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        {
            name: 'ToDate', type: 'date', hidden: false, isDefault: true, useNull: true, timeZone: +3, convert: dateConvertTimeZone,
            defaultFilterConfig:
            {
                value: Ext.create('App.extfilter.core.Range', new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 0, 0, 0, 0), null),
                operation: 'Between'
            }
        },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ProductTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ProductTreeFullPathName', type: 'string', mapping: 'ProductTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ProductTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ProductTreeObjectId', type: 'int', mapping: 'ProductTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, breezeEntityType: 'ProductTree', hidden: false, isDefault: true
        }
    ],

    proxy: {
        type: 'breeze',
        resourceName: 'NoneNegoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
    }
});
