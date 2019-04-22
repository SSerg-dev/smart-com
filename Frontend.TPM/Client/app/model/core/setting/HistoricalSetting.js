Ext.define('App.model.core.setting.HistoricalSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{ name: 'Type', type: 'string', isDefault: true },
		{ name: 'Value', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});