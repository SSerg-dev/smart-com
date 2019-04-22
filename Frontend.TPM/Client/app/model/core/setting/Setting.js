Ext.define('App.model.core.setting.Setting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Setting',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{ name: 'Type', type: 'string', isDefault: true },
		{
		    name: 'Value', type: 'string', isDefault: true, useNull: true, convert: function (value, record) {
		        if (record.get('Type') == 'System.DateTime' && value) {
		            return new Date(value);
		        }
		        return value;
		    }
		},
		{ name: 'Description', type: 'string', isDefault: true, useNull: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Settings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});