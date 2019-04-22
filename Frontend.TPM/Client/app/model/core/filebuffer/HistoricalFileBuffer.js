Ext.define('App.model.core.filebuffer.HistoricalFileBuffer', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalFileBuffer',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'InterfaceId', hidden: true },
		{ name: 'InterfaceName', type: 'string', isDefault: true, breezeEntityType: 'Interface' },
		{ name: 'InterfaceDirection', type: 'string', isDefault: true, breezeEntityType: 'Interface' },
		{ name: 'CreateDate', type: 'date', isDefault: true },
		{ name: 'UserId', hidden: true },
		{ name: 'HandlerId' },
		{ name: 'FileName', type: 'string', isDefault: true },
		{ name: 'Status', type: 'string', isDefault: true },
		{ name: 'ProcessDate', type: 'date', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalFileBuffers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});