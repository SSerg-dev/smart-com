Ext.define('App.model.core.foodintakesetting.DeletedFoodIntakeSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'FoodIntakeSetting',
    fields: [
		{ name: 'DeletedDate', type: 'date', isDefault: true },
		{ name: 'Id', hidden: true },
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
        resourceName: 'DeletedFoodIntakeSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});