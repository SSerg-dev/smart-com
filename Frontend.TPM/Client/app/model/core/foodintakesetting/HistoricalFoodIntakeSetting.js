Ext.define('App.model.core.foodintakesetting.HistoricalFoodIntakeSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalFoodIntakeSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'Number', type: 'int', isDefault: true },
		{ name: 'Type', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true },
		{ name: 'Column1', type: 'float', isDefault: true },
		{ name: 'Column2', type: 'float', isDefault: true },
		{ name: 'Column3', type: 'float', isDefault: true },
		{ name: 'Column4', type: 'float', isDefault: true },
		{ name: 'Column5', type: 'float', isDefault: true },
		{ name: 'Column6', type: 'float', isDefault: true },
		{ name: 'Column7', type: 'float', isDefault: true },
		{ name: 'Column8_1', type: 'float', isDefault: true },
		{ name: 'Column8_2', type: 'float', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalFoodIntakeSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});