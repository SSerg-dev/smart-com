Ext.define('App.model.tpm.segment.DeletedSegment', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Segment',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedSegments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
