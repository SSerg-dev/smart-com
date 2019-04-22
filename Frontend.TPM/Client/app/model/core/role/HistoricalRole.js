Ext.define('App.model.core.role.HistoricalRole', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalRole',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'SystemName', type: 'string', isDefault: true },
		{ name: 'DisplayName', type: 'string', isDefault: true },
		{ name: 'IsAllow', type: 'boolean', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});