﻿Ext.define('App.model.tpm.event.HistoricalEvent', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Event',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Description', type: 'string', hidden: false, isDefault: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'MarketSegment', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalEvents',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
