Ext.define('App.model.core.interface.HistoricalInterface', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalInterface',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true},
		{ name: '_User', type: 'string', isDefault: true},
		{ name: '_Role', type: 'string', isDefault: true},
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
		{ name: '_Operation', type: 'string', isDefault: true},
		{ name: 'Name', type: 'string', isDefault: true},
		{ name: 'Direction', type: 'string', isDefault: true},
		{ name: 'Description', type: 'string', isDefault: true}
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalInterfaces',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});