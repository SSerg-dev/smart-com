Ext.define('App.model.tpm.segment.Segment', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Segment',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Segments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
