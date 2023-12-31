﻿Ext.define('App.model.tpm.event.Event', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Event',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'EventTypeId', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'Description', type: 'string', hidden: false, isDefault: true, useNull: true },
        { name: 'MarketSegment', type: 'string', hidden: false, isDefault: true },
        { name: 'EventTypeName', type: 'string', mapping: 'EventType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'EventType', hidden: false, isDefault: true },
        { name: 'EventTypeNational', type: 'boolean', mapping: 'EventType.National', defaultFilterConfig: { valueField: 'National' }, breezeEntityType: 'EventType', hidden: true, isDefault: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Events',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            clientTreeKeyId: null
        }
    },
    
});
