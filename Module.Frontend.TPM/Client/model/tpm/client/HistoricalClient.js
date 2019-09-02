Ext.define('App.model.tpm.client.HistoricalClient', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Client',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'CommercialSubnetId', hidden: true, isDefault: true },
        { name: 'CommercialSubnetCommercialNetName', type: 'string' },
        { name: 'CommercialSubnetName', type: 'string' }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalClients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
