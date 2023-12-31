﻿Ext.define('App.model.tpm.nonenego.HistoricalNoneNego', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalNoneNego',

    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'MechanicId', hidden: true, isDefault: true },
        { name: 'MechanicTypeId', hidden: true, isDefault: true, useNull: true, defaultValue: null },
        { name: 'MechanicName', type: 'string', hidden: false, isDefault: true },
        { name: 'MechanicTypeName', type: 'string', hidden: false, isDefault: true },
        { name: 'Discount', type: 'float', hidden: false, isDefault: true },
        { name: 'FromDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ToDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'ProductTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ProductTreeId', hidden: true, isDefault: true }
    ],

    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalNoneNegoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
