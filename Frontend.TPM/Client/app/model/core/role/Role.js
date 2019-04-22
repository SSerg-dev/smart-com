Ext.define('App.model.core.role.Role', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Role',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'SystemName', type: 'string', isDefault: true },
		{ name: 'DisplayName', type: 'string', isDefault: true },
		{ name: 'IsAllow', type: 'boolean', isDefault: true, defaultValue: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Roles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});