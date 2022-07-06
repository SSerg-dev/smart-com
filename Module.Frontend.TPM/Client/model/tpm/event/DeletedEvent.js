Ext.define('App.model.tpm.event.DeletedEvent', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Event',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'MarketSegment', type: 'string', hidden: false, isDefault: true },
        { name: 'EventTypeName', type: 'string', mapping: 'EventType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'EventType', hidden: false, isDefault: true },
        { name: 'EventTypeNational', type: 'boolean', mapping: 'EventType.National', defaultFilterConfig: { valueField: 'National' }, breezeEntityType: 'EventType', hidden: true, isDefault: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedEvents',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
