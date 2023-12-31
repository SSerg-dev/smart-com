﻿Ext.define('App.model.tpm.program.HistoricalProgram', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Program',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPrograms',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
