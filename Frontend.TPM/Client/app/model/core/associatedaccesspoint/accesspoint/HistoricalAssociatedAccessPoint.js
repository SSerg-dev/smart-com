Ext.define('App.model.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalAccessPoint',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'Resource', type: 'string', isDefault: true },
		{ name: 'Action', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalAccessPoints',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});