Ext.define('App.model.tpm.metricslivehistory.MetricsLiveHistory', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'MetricsLiveHistory',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true, useNull: false },
        { name: 'Date', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ClientTreeId', type: 'int', hidden: false, isDefault: true, useNull: false },
        { name: 'Value', type: 'float', hidden: false, isDefault: true, useNull: false },
        { name: 'ValueLSV', type: 'float', hidden: false, isDefault: true, useNull: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'MetricsLiveHistories',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
