Ext.define('App.model.tpm.durationrange.DurationRange', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'DurationRange',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'MinValue', type: 'int', hidden: false, isDefault: true },
        { name: 'MaxValue', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DurationRanges',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
